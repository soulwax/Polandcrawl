using UnityEngine;
using System;

public class LevelGenerator {
	public static Gaussian random = new Gaussian();
	public double[] values;
	private int w, h;

	public LevelGenerator(int w, int h, int featureSize) {
		this.w = w;
		this.h = h;

		values = new double[w * h];

		for (int y = 0; y < w; y += featureSize) {
			for (int x = 0; x < w; x += featureSize) {
				setSample(x, y, random.NextFloat() * 2 - 1);
			}
		}

		int stepSize = featureSize;
		double scale = 1.0 / w;
		double scaleMod = 1;
		do {
			int halfStep = stepSize / 2;
			for (int y = 0; y < w; y += stepSize) {
				for (int x = 0; x < w; x += stepSize) {
					double a = sample(x, y);
					double b = sample(x + stepSize, y);
					double c = sample(x, y + stepSize);
					double d = sample(x + stepSize, y + stepSize);

					double e = (a + b + c + d) / 4.0 + (random.NextFloat() * 2 - 1) * stepSize * scale;
					setSample(x + halfStep, y + halfStep, e);
				}
			}
			for (int y = 0; y < w; y += stepSize) {
				for (int x = 0; x < w; x += stepSize) {
					double a = sample(x, y);
					double b = sample(x + stepSize, y);
					double c = sample(x, y + stepSize);
					double d = sample(x + halfStep, y + halfStep);
					double e = sample(x + halfStep, y - halfStep);
					double f = sample(x - halfStep, y + halfStep);

					double H = (a + b + d + e) / 4.0 + (random.NextFloat() * 2 - 1) * stepSize * scale * 0.5;
					double g = (a + c + d + f) / 4.0 + (random.NextFloat() * 2 - 1) * stepSize * scale * 0.5;
					setSample(x + halfStep, y, H);
					setSample(x, y + halfStep, g);
				}
			}
			stepSize /= 2;
			scale *= (scaleMod + 0.8);
			scaleMod *= 0.3;
		} while (stepSize > 1);
	}

	private double sample(int x, int y) {
		return values[(x & (w - 1)) + (y & (h - 1)) * w];
	}

	private void setSample(int x, int y, double value) {
		values[(x & (w - 1)) + (y & (h - 1)) * w] = value;
	}

	public static byte[][,] CreateAndValidateUndergroundMap(int w, int h, int depth) {
//		int attempt = 0;
		do {
			byte[][,] result = createUndergroundMap(w, h, depth);

			int[] count = new int[256];

			for(int y = 0; y < h; y++) {
				for(int x = 0; x < w; x++) {
					count[result[0][x,y] & 0xff]++;
				}
			}

			if (count[Tile.rock.id & 0xff] < 100) continue;
			if (count[Tile.dirt.id & 0xff] < 100) continue;
			if (count[Tile.ironOre.id & 0xff] < 20) continue;
			//if (depth < 3) if (count[Tile.stairsDown.id & 0xff] < 2) continue;

			return result;

		} while (true);
	}

	private static byte[][,] createUndergroundMap(int w, int h, int depth) {
		LevelGenerator mnoise1 = new LevelGenerator(w, h, 16);
		LevelGenerator mnoise2 = new LevelGenerator(w, h, 16);
		LevelGenerator mnoise3 = new LevelGenerator(w, h, 16);

		LevelGenerator nnoise1 = new LevelGenerator(w, h, 16);
		LevelGenerator nnoise2 = new LevelGenerator(w, h, 16);
		LevelGenerator nnoise3 = new LevelGenerator(w, h, 16);

		LevelGenerator wnoise1 = new LevelGenerator(w, h, 16);
		LevelGenerator wnoise2 = new LevelGenerator(w, h, 16);
		LevelGenerator wnoise3 = new LevelGenerator(w, h, 16);

		LevelGenerator noise1 = new LevelGenerator(w, h, 32);
		LevelGenerator noise2 = new LevelGenerator(w, h, 32);

		byte[,] map = new byte[w , h];
		byte[,] data = new byte[w , h];
		for (int y = 0; y < h; y++) {
			for (int x = 0; x < w; x++) {
				int i = x + y * w;

				double val = Math.Abs(noise1.values[i] - noise2.values[i]) * 3 - 2;

				double mval = Math.Abs(mnoise1.values[i] - mnoise2.values[i]);
				mval = Math.Abs(mval - mnoise3.values[i]) * 3 - 2;

				double nval = Math.Abs(nnoise1.values[i] - nnoise2.values[i]);
				nval = Math.Abs(nval - nnoise3.values[i]) * 3 - 2;

				double wval = Math.Abs(wnoise1.values[i] - wnoise2.values[i]);
				wval = Math.Abs(nval - wnoise3.values[i]) * 3 - 2;

				double xd = x / (w - 1.0) * 2 - 1;
				double yd = y / (h - 1.0) * 2 - 1;
				if (xd < 0) xd = -xd;
				if (yd < 0) yd = -yd;
				double dist = xd >= yd ? xd : yd;
				dist = dist * dist * dist * dist;
				dist = dist * dist * dist * dist;
				val = val + 1 - dist * 20;

				if (val > -2 && wval < -2.0 + (depth) / 2 * 3) {
					if (depth > 2)
						map[x,y] = Tile.lava.id;
					else
						map[x,y] = Tile.water.id;
				} else if (val > -2 && (mval < -1.7 || nval < -1.4)) {
					map[x,y] = Tile.dirt.id;
				} else {
					map[x,y] = Tile.rock.id;
				}
			}
		}

		{
			int r = 2;
			for (int i = 0; i < w * h / 400; i++) {
				int x = random.NextInt(w);
				int y = random.NextInt(h);
				for (int j = 0; j < 30; j++) {
					int xx = x + random.NextInt(5) - random.NextInt(5);
					int yy = y + random.NextInt(5) - random.NextInt(5);
					if (xx >= r && yy >= r && xx < w - r && yy < h - r) {
						if (map[xx,yy] == Tile.rock.id) {
							map[xx,yy] = (byte) (Tile.ironOre.id & 0xff);
						}
					}
				}
			}
		}

		if (depth < 3) {
			int yy, xx, i;
			int count = 0;
			 for (i = 0; i < w * h / 100; i++) {
				int x = random.NextInt(w - 20) + 10;
				int y = random.NextInt(h - 20) + 10;

				for (yy = y - 1; yy <= y + 1; yy++)
					for (xx = x - 1; xx <= x + 1; xx++) {
						if (map[xx,yy] != Tile.rock.id) {
							i++;
							map[x,y] = Tile.stairsDown.id;
						}
					}

				map[x,y] = Tile.stairsDown.id;
				count++;
				if (count == 4) break;
			}
		}

		return new byte[][,] { map, data };
	}
}