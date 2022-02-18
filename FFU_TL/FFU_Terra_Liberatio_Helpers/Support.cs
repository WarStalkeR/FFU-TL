using CoOpSpRpG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System;

namespace FFU_Terra_Liberatio {
    public static class Support {
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
				try { tStream.Dispose(); } catch { }
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
				try { tStream.Dispose(); } catch { }
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
			return PatchTexture(mTex, TextureFromStream(tPatch.lightHex), 
				tPatch.xOffset, tPatch.yOffset, tPatch.tResolution, true, Color.Black, Background.transparent);
		}
		public static Texture2D PatchSheet(Texture2D mTex, TexturePatch tPatch) {
			return PatchTexture(mTex, TextureFromStream(tPatch.artHex), 
				tPatch.xOffset, tPatch.yOffset, tPatch.tResolution, true, Color.Transparent, Background.transparent);
		}
		public static Texture2D PatchTexture(Texture2D mTex, TexturePatch tPatch) {
			return PatchTexture(PatchTexture(mTex,
			TextureFromStream(tPatch.artHex), tPatch.xOffset, tPatch.yOffset, tPatch.tResolution, true, Color.Transparent, Background.transparent),
			TextureFromStream(tPatch.emitHex), tPatch.xOffset + 128, tPatch.yOffset, tPatch.tResolution, true, Color.Black, Background.black);
		}
		public static Texture2D PatchTexture(Texture2D mTex, Texture2D pTex, int sX, int sY, int tRes, bool vTile, Color fColor, Background bType) {
			ModLog.Message($"Patching Texture: {mTex.Name}...");
			try {
				int patchX = pTex.Width / tRes;
				int patchY = pTex.Height / tRes;
				int patchN = pTex.Height * pTex.Width / tRes;
				int refWidth = Math.Max(mTex.Width, (sX + patchX) * tRes);
				int refHeight = Math.Max(mTex.Height, (sY + patchY) * tRes);
				int tilesX = refWidth / tRes;
				int tilesN = refHeight * refWidth / tRes;
				Color[] mTexStream = new Color[mTex.Height * mTex.Width];
				Color[] pTexStream = new Color[pTex.Height * pTex.Width];
				mTex.GetData(mTexStream);
				pTex.GetData(pTexStream);
				Color[,] ref2D = Make2DArray(mTexStream, mTex.Height, mTex.Width);
				Color[,] patch2D = Make2DArray(pTexStream, pTex.Height, pTex.Width);
				if (refWidth > mTex.Width || refHeight > mTex.Height)
					ref2D = Resize2DArray(ref2D, refHeight, refWidth, fColor);
				Parallel.For(0, tilesN, tN => {
					int tX = tN % tilesX;
					int tY = tN / tilesX;
					if (tX >= sX && tY >= sY
						&& tX < (sX + patchX)
						&& tY < (sY + patchY)) {
						Point currTile = new Point(tX - sX, tY - sY);
						bool emptyTile = vTile ? IsEmptyTile(currTile, patch2D, tRes, bType) : false;
						if (!vTile || !emptyTile) {
							Parallel.For(0, tRes * tRes, rN => {
								int rX = tX * tRes + (rN % tRes);
								int rY = tY * tRes + (rN / tRes);
								int pX = rX - sX * tRes;
								int pY = rY - sY * tRes;
								try {
									ref2D[rY, rX] = patch2D[pY, pX];
								} catch {
									ModLog.Fatal($"PatchTexture Failed! X:{rX}, Y:{rY}, pX:{pX}, pY:{pY}");
								}
							});
						}
					}
				});
				Color[] refTextStream = Make1DArray(ref2D);
				Texture2D rTex = new Texture2D(SCREEN_MANAGER.Device, refWidth, refHeight);
				rTex.SetData(refTextStream);
				if (!string.IsNullOrEmpty(mTex.Name)) rTex.Name = mTex.Name + " P";
				try { mTex.Dispose(); } catch { }
				try { pTex.Dispose(); } catch { }
				if (rTex != null) return rTex;
				return null;
			} catch (Exception ex) {
				ModLog.Fatal($"Couldn't patch texture via stream! Exception: {ex}");
				return null;
			}
		}
		public static MemoryStream StreamFromTexture(Texture2D rTex, bool noAlpha) {
			try {
				Color[] cData = new Color[rTex.Height * rTex.Width];
				rTex.GetData(cData);
                System.Windows.Media.PixelFormat rFormat = System.Windows.Media.PixelFormats.Bgra32;
				byte[] cArray = new byte[cData.Length * 4];
				int arrPos = 0;
				for (int i = 0; i < cData.Length; i++) {
					cArray[arrPos] = cData[i].B; arrPos++;
					cArray[arrPos] = cData[i].G; arrPos++;
					cArray[arrPos] = cData[i].R; arrPos++;
					cArray[arrPos] = noAlpha ? (byte)255 : cData[i].A; arrPos++;
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
		public static void DumpImageToFile(Texture2D rImage, string dumpFile = "Dumped_Image.png", bool noAlpha = false) {
			ModLog.Warning($"Dumping 2D texture into the {dumpFile}...");
			ValidateDirPath(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir);
			BinaryWriter imgDump = new BinaryWriter(File.OpenWrite(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modDumpsDir + dumpFile));
			MemoryStream imgStream = StreamFromTexture(rImage, noAlpha);
			imgDump.Write(imgStream.ToArray());
			imgDump.Close();
			try { imgDump.Dispose(); } catch { }
			try { imgStream.Dispose(); } catch { }
		}
		public static bool IsEmptyTile(Point cTile, Color[,] refTex, int tRes, Background bType) {
			for (int y = 0; y < tRes; y++)
				for (int x = 0; x < tRes; x++) {
					try {
						Color refPixel = refTex[cTile.Y * tRes + y, cTile.X * tRes + x];
						switch (bType) {
							case Background.transparent: if (!IsTransparent(refPixel)) return false; break;
							case Background.black: if (!IsPureBlack(refPixel)) return false; break;
							case Background.special: if (!IsColorSpecial(refPixel)) return false; break;
						}
					} catch {
						ModLog.Fatal($"IsEmptyTile Failed! X:{x}, Y:{y}, rX:{cTile.X * tRes + x}, rY:{cTile.Y * tRes + y}");
						return false;
					}
				}
			return true;
        }
		public static bool IsTransparent(Color rPixel) {
			return rPixel.A == 0;
		}
		public static bool IsPureBlack(Color rPixel) {
			return rPixel.R == 0 && rPixel.G == 0 && rPixel.B == 0 && rPixel.A == 255;
		}
		public static bool IsColorSpecial(Color rPixel) {
			return rPixel.R == 255 && rPixel.G == 0 && rPixel.B == 255 && rPixel.A == 255;
		}
		public static void ValidateDirPath(string dirPath) {
			if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
		}
		public static T[,] Resize2DArray<T>(T[,] tInput, int nHeight, int nWidth, T tFallback) {
			var tOutput = new T[nHeight, nWidth];
			int oHeight = tInput.GetLength(0);
			int oWidth = tInput.GetLength(1);
			for (int y = 0; y < nHeight; y++)
				for (int x = 0; x < nWidth; x++)
					if (y < oHeight && x < oWidth) tOutput[y, x] = tInput[y, x];
					else tOutput[y, x] = tFallback;
			return tOutput;
		}
		public static T[,] Make2DArray<T>(T[] tInput, int tHeight, int tWidth) {
			T[,] tOutput = new T[tHeight, tWidth];
			for (int y = 0; y < tHeight; y++) {
				for (int x = 0; x < tWidth; x++) {
					tOutput[y, x] = tInput[y * tWidth + x];
				}
			}
			return tOutput;
		}
		public static T[] Resize1DArray<T>(T[] tInput, int oHeight, int oWidth, int nHeight, int nWidth, T tFallback) {
			var tOutput = new T[nHeight, nWidth];
			var tInput2D = Make2DArray(tInput, oHeight, oWidth);
			for (int y = 0; y < nHeight; y++)
				for (int x = 0; x < nWidth; x++)
					if (y < oHeight && x < oWidth) tOutput[y, x] = tInput2D[y, x];
					else tOutput[y, x] = tFallback;
			return Make1DArray(tOutput);
		}
		public static T[] Make1DArray<T>(T[,] tInput) {
			int oHeight = tInput.GetLength(0);
			int oWidth = tInput.GetLength(1);
			var tOutput = new T[oHeight * oWidth];
			for (int y = 0; y < oHeight; y++)
				for (int x = 0; x < oWidth; x++) 
					tOutput[y * oWidth + x] = tInput[y, x];
			return tOutput;
        }
		public enum Background {
			transparent,
			black,
			special
		}
	}
}
