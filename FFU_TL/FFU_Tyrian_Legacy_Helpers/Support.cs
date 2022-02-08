using CoOpSpRpG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace FFU_Tyrian_Legacy {
    public static class Support {
		public static bool emptyTile = false;
		public static Point prevTile = new Point(int.MaxValue, int.MaxValue);
		public static Point currTile = new Point(int.MaxValue, int.MaxValue);
		public static BinaryReader StringToBinStream(string hexStr) {
			return new BinaryReader(StringToMemStream(hexStr));
		}
		public static MemoryStream StringToMemStream(string hexStr) {
			return new MemoryStream(StringToByteStream(hexStr));
		}
		public static byte[] StringToByteStream(string hexStr) {
            return Enumerable.Range(0, hexStr.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hexStr.Substring(x, 2), 16))
            .ToArray();
        }
		public static Texture2D TextureFromStream(string hexStr) {
			return TextureFromStream(StringToMemStream(hexStr));
		}
        public static Texture2D TextureFromStream(MemoryStream tStream) {
			try {
				BitmapSource bSource = new PngBitmapDecoder(tStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
				int pWidth = bSource.PixelWidth;
				int pHeight = bSource.PixelHeight;
				int clrArrSize = pWidth * pHeight;
				int hexArrSize = pWidth * (bSource.Format.BitsPerPixel / 8);
				byte[] bArr = new byte[pHeight * hexArrSize];
				bSource.CopyPixels(bArr, hexArrSize, 0);
				int clrOffset = bSource.Format.BitsPerPixel / 8;
				Color[] clrArray = new Color[clrArrSize];
				for (int i = 0; i < clrArrSize; i++) {
					byte b = bArr[i * clrOffset];
					byte g = bArr[i * clrOffset + 1];
					byte r = bArr[i * clrOffset + 2];
					if (clrOffset > 3) clrArray[i] = new Color(r, g, b, bArr[i * 4 + 3]);
					else clrArray[i] = new Color(r, g, b, byte.MaxValue);
				}
				Texture2D rTex = new Texture2D(SCREEN_MANAGER.Device, pWidth, pHeight);
				rTex.SetData(clrArray);
				if (rTex != null) return rTex;
				return null;
			} catch (Exception ex) {
				ModLog.Fatal($"Couldn't create texture from steam! Exception: {ex}");
				return null;
			}
		}
		public static Color[] ColorFromStream(string hexStr) {
			return ColorFromStream(StringToMemStream(hexStr));
		}
		public static Color[] ColorFromStream(MemoryStream tStream) {
			try {
				BitmapSource bSource = new PngBitmapDecoder(tStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
				int pWidth = bSource.PixelWidth;
				int pHeight = bSource.PixelHeight;
				int clrArrSize = pWidth * pHeight;
				int hexArrSize = pWidth * (bSource.Format.BitsPerPixel / 8);
				byte[] bArr = new byte[pHeight * hexArrSize];
				bSource.CopyPixels(bArr, hexArrSize, 0);
				int clrOffset = bSource.Format.BitsPerPixel / 8;
				Color[] clrArray = new Color[clrArrSize];
				for (int i = 0; i < clrArrSize; i++) {
					byte b = bArr[i * clrOffset];
					byte g = bArr[i * clrOffset + 1];
					byte r = bArr[i * clrOffset + 2];
					if (clrOffset > 3) clrArray[i] = new Color(r, g, b, bArr[i * 4 + 3]);
					else clrArray[i] = new Color(r, g, b, byte.MaxValue);
				}
				return clrArray;
			} catch (Exception ex) {
				ModLog.Fatal($"Couldn't create color array from steam! Exception: {ex}");
				return null;
			}
		}
		public static ModTile[] PatchTiles(TexturePatch tPatch, Color refColor) {
			Color[,] tilesMap = Make2DArray(ColorFromStream(tPatch.tilesHex), tPatch.tHeight, tPatch.tWidth);
			Color[,] blocksMap = Make2DArray(ColorFromStream(tPatch.blocksHex), tPatch.tHeight, tPatch.tWidth);
			Color[,] airwayMap = Make2DArray(ColorFromStream(tPatch.airwayHex), tPatch.tHeight, tPatch.tWidth);
			Color[,] repairMap = Make2DArray(ColorFromStream(tPatch.repairHex), tPatch.tHeight, tPatch.tWidth);
			uint tNum = 0;
			int mainTilePosX = 0;
			int mainTilePosY = 0;
			bool mainSet = false;
			for (int y = 0; y < tPatch.tHeight; y++) {
				for (int x = 0; x < tPatch.tWidth; x++) {
					if (tilesMap[y, x] == refColor) {
						if (!mainSet) {
							mainTilePosX = x;
							mainTilePosY = y;
							mainSet = true;
                        }
						tNum++;
					}
				}
			}
			ModTile[] refTiles = new ModTile[tNum];
			tNum = 0;
			for (int y = 0; y < tPatch.tHeight; y++) {
				for (int x = 0; x < tPatch.tWidth; x++) {
					if (tilesMap[y, x] == refColor) {
						refTiles[tNum] = new ModTile();
						refTiles[tNum].X = (x - mainTilePosX) * 16;
						refTiles[tNum].Y = (y - mainTilePosY) * 16;
						refTiles[tNum].U = (x + tPatch.xOffset) * 16;
						refTiles[tNum].V = (y + tPatch.yOffset) * 16;
						refTiles[tNum].inside = blocksMap[y, x].R > 0;
						refTiles[tNum].preferOutside = blocksMap[y, x].G > 0;
						refTiles[tNum].blocking = blocksMap[y, x].B > 0;
						refTiles[tNum].airBlocking = airwayMap[y, x].R > 0;
						refTiles[tNum].connectUp = airwayMap[y, x].G > 0;
						refTiles[tNum].connectDown = airwayMap[y, x].B > 0;
						refTiles[tNum].connectLeft = repairMap[y, x].R > 0;
						refTiles[tNum].connectRight = repairMap[y, x].G > 0;
						refTiles[tNum].repairable = repairMap[y, x].B > 0;
						tNum++;
					}
				}
			}
			return refTiles;
        }
		public static Texture2D PatchLight(Texture2D mTex, TexturePatch tPatch) {
			return PatchTexture(mTex, TextureFromStream(tPatch.lightHex), tPatch.xOffset, tPatch.yOffset, 2, false);
		}
		public static Texture2D PatchTexture(Texture2D mTex, TexturePatch tPatch) {
			return PatchTexture(PatchTexture(mTex,
			TextureFromStream(tPatch.artHex), tPatch.xOffset, tPatch.yOffset, 16, true),
			TextureFromStream(tPatch.emitHex), tPatch.xOffset + 128, tPatch.yOffset, 16, true);
		}
		public static Texture2D PatchTexture(Texture2D mTex, string pTex, int sX, int sY, int tRes, bool vTile) {
			return PatchTexture(mTex, TextureFromStream(pTex), sX, sY, tRes, vTile);
		}
		public static Texture2D PatchTexture(Texture2D mTex, Texture2D pTex, int sX, int sY, int tRes, bool vTile) {
			ModLog.Message($"Patching Texture: {mTex.Name}...");
			try {
				int tilesX = mTex.Width / tRes;
				int tilesY = mTex.Height / tRes;
				int patchX = pTex.Width / tRes;
				int patchY = pTex.Height / tRes;
				Color[] mTexStream = new Color[mTex.Height * mTex.Width];
				Color[] pTexStream = new Color[pTex.Height * pTex.Width];
				mTex.GetData(mTexStream);
				pTex.GetData(pTexStream);
				Color[,] ref2D = Make2DArray(mTexStream, mTex.Height, mTex.Width);
				Color[,] patch2D = Make2DArray(pTexStream, pTex.Height, pTex.Width);
				for (int tY = 0; tY < tilesY; tY++) {
					for (int tX = 0; tX < tilesX; tX++) {
						if (tX >= sX && tY >= sY
						&& tX < (sX + patchX)
						&& tY < (sY + patchY)) {
							currTile = new Point(tX - sX, tY - sY);
							if (vTile && prevTile != currTile) {
								prevTile = currTile;
								emptyTile = IsEmptyTile(currTile, patch2D, tRes);
								//ModLog.Warning($"Tile X:{tX - sX}, Y:{tY - sY}, Empty:{emptyTile}");
							}
							if (!vTile || !emptyTile) {
								for (int rY = 0; rY < tRes; rY++) {
									for (int rX = 0; rX < tRes; rX++) {
										int x = tX * tRes + rX;
										int y = tY * tRes + rY;
										int xP = x - sX * tRes;
										int yP = y - sY * tRes;
										try {
											ref2D[y, x] = patch2D[yP, xP];
										} catch {
											ModLog.Fatal($"PatchTexture Failed! X:{x}, Y:{y}, pX:{xP}, pY:{yP}");
										}
									}
								}
							}
						}
					}
                }
				ResetTileVariables();
				Color[] refTextStream = Make1DArray(ref2D);
				Texture2D rTex = new Texture2D(SCREEN_MANAGER.Device, mTex.Width, mTex.Height);
				rTex.SetData(refTextStream);
				if (!string.IsNullOrEmpty(mTex.Name)) rTex.Name = mTex.Name + " P";
				if (rTex != null) return rTex;
				return null;
			} catch (Exception ex) {
				ModLog.Fatal($"Couldn't patch texture via stream! Exception: {ex}");
				return null;
			}
		}
		public static MemoryStream StreamFromTexture(Texture2D rTex) {
			try {
				Color[] cData = new Color[rTex.Height * rTex.Width];
				rTex.GetData(cData);
                System.Windows.Media.PixelFormat rFormat = System.Windows.Media.PixelFormats.Bgra32;
				byte[] cArray = new byte[cData.Length * 4];
				int arrPos = 0;
				for (int i = 0; i < cData.Length; i++) {
					cArray[arrPos] = cData[i].B;
					arrPos++;
					cArray[arrPos] = cData[i].G;
					arrPos++;
					cArray[arrPos] = cData[i].R;
					arrPos++;
					cArray[arrPos] = cData[i].A;
					arrPos++;
				}
				BitmapSource source = BitmapSource.Create(rTex.Width, rTex.Height, 96.0, 96.0, rFormat, null, cArray, rTex.Width * 4);
				MemoryStream memoryStream = new MemoryStream();
				PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
				pngBitmapEncoder.Interlace = PngInterlaceOption.Off;
				pngBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
				pngBitmapEncoder.Save(memoryStream);
				return memoryStream;
			} catch (Exception ex) {
				ModLog.Fatal($"Couldn't create stream from texture! Exception: {ex}");
				return null;
			}
		}
		public static void DumpImageToFile(Texture2D rImage, string dumpFile = "Dumped_Image.png") {
			ModLog.Warning($"Dumping 2D texture into the {dumpFile}...");
			ValidateDirPath(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir);
			BinaryWriter imgDump = new BinaryWriter(File.OpenWrite(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir + dumpFile));
			imgDump.Write(StreamFromTexture(rImage).ToArray());
			imgDump.Close();
			imgDump.Dispose();
		}
		public static bool IsEmptyTile(Point cTile, Color[,] refTex, int tRes) {
			for (int y = 0; y < tRes; y++)
				for (int x = 0; x < tRes; x++) {
					try {
						Color refPixel = refTex[cTile.Y * tRes + y, cTile.X * tRes + x];
						if (refPixel.A > 0) return false;
					} catch {
						ModLog.Fatal($"IsEmptyTile Failed! X:{x}, Y:{y}, rX:{cTile.X * tRes + x}, rY:{cTile.Y * tRes + y}");
						return false;
					}
				}
			return true;
        }
		public static void ResetTileVariables() {
			prevTile = new Point(int.MaxValue, int.MaxValue);
			currTile = new Point(int.MaxValue, int.MaxValue);
			emptyTile = false;
		}
		public static void ValidateDirPath(string dirPath) {
			if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
		}
		public static T[,] Make2DArray<T>(T[] input, int height, int width) {
			T[,] output = new T[height, width];
			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					output[i, j] = input[i * width + j];
				}
			}
			return output;
		}
		public static T[] Make1DArray<T>(T[,] input) {
			return input.Cast<T>().ToArray();
        }
	}
}
