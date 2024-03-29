﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CsharpRAPL.Benchmarking;
using CsharpRAPL.Benchmarking.Attributes;

namespace Benchmarks.Loops;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class LinqBenchmarks {
	public static ulong Iterations;
	public static ulong LoopIterations;


	private static readonly ulong[] RandomValues = {
		132, 700, 206, 791, 374, 456, 597, 758, 477, 321, 30, 96, 842, 568, 337, 189, 640, 551, 570, 501, 893, 974, 554,
		461, 640, 276, 256, 929, 725, 624, 288, 658, 828, 462, 153, 991, 3, 242, 583, 908, 349, 753, 436, 645, 574, 807,
		302, 242, 760, 514, 920, 468, 709, 703, 209, 152, 638, 674, 818, 541, 359, 353, 527, 216, 257, 158, 81, 630,
		258, 218, 480, 613, 454, 788, 162, 913, 992, 375, 319, 515, 56, 791, 672, 229, 693, 122, 894, 418, 563, 652,
		674, 771, 159, 768, 273, 866, 894, 125, 508, 964, 119, 882, 584, 816, 331, 156, 111, 159, 578, 684, 467, 798,
		159, 859, 549, 919, 859, 392, 329, 228, 892, 838, 411, 88, 952, 44, 178, 356, 106, 979, 554, 589, 475, 802, 198,
		732, 672, 261, 328, 450, 147, 644, 960, 623, 703, 507, 613, 452, 501, 923, 785, 171, 367, 996, 318, 831, 255,
		288, 724, 467, 818, 30, 406, 172, 460, 901, 78, 699, 968, 495, 538, 526, 935, 154, 666, 48, 115, 956, 982, 971,
		882, 4, 182, 347, 844, 173, 842, 738, 215, 957, 456, 623, 370, 261, 861, 366, 191, 517, 850, 212, 788, 818, 382,
		625, 892, 324, 92, 794, 675, 834, 595, 488, 582, 535, 236, 406, 808, 513, 81, 902, 404, 883, 933, 469, 221, 70,
		118, 466, 392, 652, 244, 82, 660, 489, 354, 148, 395, 551, 64, 124, 132, 153, 37, 955, 960, 702, 0, 15, 219,
		902, 474, 270, 484, 734, 957, 174, 103, 352, 529, 871, 288, 254, 854, 284, 843, 287, 599, 705, 348, 453, 648,
		652, 777, 569, 785, 861, 97, 756, 654, 702, 590, 420, 833, 788, 487, 433, 794, 900, 879, 585, 971, 871, 544,
		718, 389, 553, 298, 884, 184, 471, 733, 496, 835, 314, 975, 54, 307, 945, 461, 992, 819, 513, 281, 50, 457, 939,
		445, 31, 197, 889, 473, 658, 947, 453, 45, 375, 980, 676, 173, 36, 286, 248, 859, 13, 41, 271, 444, 540, 377,
		347, 262, 997, 816, 749, 814, 388, 708, 839, 866, 913, 430, 214, 316, 401, 960, 334, 458, 847, 938, 147, 328,
		592, 849, 206, 890, 332, 604, 704, 78, 129, 817, 98, 115, 471, 195, 711, 368, 981, 622, 213, 685, 912, 97, 943,
		915, 257, 258, 824, 485, 596, 26, 64, 659, 758, 255, 677, 170, 494, 203, 963, 80, 470, 817, 347, 426, 338, 523,
		33, 550, 942, 420, 283, 377, 791, 750, 125, 361, 518, 118, 220, 287, 112, 640, 120, 406, 87, 498, 883, 620, 719,
		235, 339, 280, 996, 672, 639, 130, 463, 691, 978, 89, 84, 867, 937, 585, 64, 974, 549, 934, 600, 599, 235, 965,
		227, 326, 664, 395, 69, 519, 583, 368, 776, 164, 440, 393, 908, 959, 881, 578, 342, 214, 569, 439, 64, 264, 851,
		325, 846, 929, 592, 598, 802, 447, 560, 514, 101, 92, 272, 939, 977, 302, 803, 94, 216, 615, 933, 513, 960, 447,
		475, 198, 923, 875, 851, 566, 100, 27, 705, 510, 363, 15, 847, 410, 410, 874, 860, 957, 247, 973, 433, 184, 922,
		595, 265, 759, 58, 623, 970, 432, 919, 799, 454, 885, 246, 821, 689, 808, 945, 672, 58, 393, 214, 713, 634, 757,
		94, 530, 222, 948, 820, 167, 390, 152, 276, 295, 884, 88, 512, 258, 869, 687, 865, 395, 445, 304, 13, 183, 71,
		263, 914, 972, 117, 252, 128, 999, 942, 257, 993, 799, 555, 129, 892, 685, 238, 644, 196, 37, 525, 52, 104, 869,
		751, 808, 950, 233, 294, 512, 502, 396, 554, 826, 729, 792, 66, 302, 902, 674, 797, 6, 579, 92, 590, 330, 644,
		464, 524, 286, 958, 273, 153, 850, 540, 517, 881, 616, 751, 609, 1000, 898, 828, 799, 523, 976, 838, 786, 755,
		468, 91, 855, 555, 906, 986, 993, 835, 163, 577, 792, 629, 674, 375, 440, 205, 449, 777, 691, 667, 634, 677,
		382, 687, 520, 636, 767, 737, 565, 210, 160, 388, 480, 230, 287, 782, 913, 713, 521, 336, 60, 442, 687, 366,
		946, 170, 403, 238, 422, 39, 917, 216, 511, 995, 206, 745, 115, 232, 898, 504, 512, 348, 8, 920, 114, 36, 593,
		455, 377, 619, 934, 415, 767, 145, 789, 113, 871, 486, 371, 390, 484, 307, 250, 914, 15, 990, 484, 18, 385, 396,
		970, 690, 829, 34, 643, 888, 772, 715, 75, 744, 247, 184, 742, 337, 62, 350, 94, 414, 574, 409, 665, 871, 982,
		251, 939, 560, 545, 715, 489, 122, 491, 554, 890, 377, 814, 572, 532, 506, 441, 128, 818, 546, 989, 615, 549,
		578, 967, 47, 909, 151, 837, 669, 760, 62, 335, 517, 878, 891, 377, 538, 185, 461, 974, 196, 455, 387, 930, 154,
		421, 734, 14, 736, 859, 791, 950, 484, 757, 228, 650, 708, 77, 106, 601, 320, 997, 371, 984, 500, 885, 910, 102,
		863, 564, 150, 199, 249, 890, 296, 492, 560, 939, 714, 323, 491, 744, 306, 826, 825, 139, 379, 754, 121, 90,
		959, 24, 28, 51, 910, 588, 10, 12, 529, 438, 913, 860, 206, 972, 532, 142, 145, 687, 301, 835, 672, 631, 4, 385,
		702, 439, 247, 850, 135, 571, 363, 432, 678, 780, 370, 847, 821, 397, 272, 984, 472, 362, 407, 129, 364, 382,
		803, 183, 592, 824, 847, 760, 8, 659, 837, 44, 541, 93, 837, 208, 464, 453, 302, 68, 989, 689, 198, 73, 62, 700,
		355, 190, 192, 608, 253, 563, 976, 155, 225, 87, 426, 846, 716, 711, 589, 966, 909, 738, 195, 770, 785, 933,
		758, 470, 88, 260, 606, 15, 437, 591, 266, 347, 169, 450, 502, 65, 205, 195, 203, 415, 824, 427, 1, 751, 690,
		258, 760, 425, 253, 251, 461, 145, 446, 269, 665, 223, 530, 489, 884, 260, 277, 338, 548, 916, 440, 69, 396,
		606, 444, 727, 958, 165, 985, 457, 579, 152, 353, 809, 299, 560, 882, 283, 970, 405, 868, 701, 454, 125, 471,
		958, 776
	};

	[Benchmark("LoopsLinq", "Tests LINQ using Aggregate to get the sum")]
	public static ulong LinqAggregate() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			result += RandomValues.Aggregate((left, right) => left + right);
		}


		return result;
	}

	[Benchmark("LoopsLinq", "Tests a foreach loop equivalent to LINQ Sum")]
	public static ulong LinqSumForEachEquivalent() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			foreach (ulong value in RandomValues) {
				result += value;
			}
		}

		return result;
	}

	[Benchmark("LoopsLinq", "Tests a for loop equivalent to LINQ Sum")]
	public static ulong LinqSumForEquivalent() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < RandomValues.Length; j++) {
				result += RandomValues[j];
			}
		}

		return result;
	}

	[Benchmark("LoopsLinq", "Tests LINQ using Min")]
	public static ulong LinqMin() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			result += RandomValues.Min();
		}


		return result;
	}

	[Benchmark("LoopsLinq", "Tests a foreach loop equivalent to LINQ Min")]
	public static ulong LinqMinForEachEquivalent() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ulong min = 0;
			foreach (ulong value in RandomValues) {
				if (value < min) {
					min = value;
				}
			}

			result += min;
		}

		return result;
	}

	[Benchmark("LoopsLinq", "Tests a for loop equivalent to LINQ Min")]
	public static ulong LinqMinForEquivalent() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			ulong min = 0;
			for (int j = 0; j < RandomValues.Length; j++) {
				ulong value = RandomValues[j];
				if (value < min) {
					min = value;
				}
			}

			result += min;
		}

		return result;
	}

	[Benchmark("LoopsLinq", "Tests LINQ using Select Aggregate to get the sum")]
	public static ulong LinqSelectAggregate() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			result += RandomValues.Select(value => value + value + 1).Aggregate((left, right) => left + right);
		}


		return result;
	}

	[Benchmark("LoopsLinq", "Tests a foreach loop equivalent to LINQ Select Sum")]
	public static ulong LinqSelectSumForEachEquivalent() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			foreach (ulong value in RandomValues) {
				result += value + value + 1;
			}
		}

		return result;
	}

	[Benchmark("LoopsLinq", "Tests a for loop equivalent to LINQ Select Sum")]
	public static ulong LinqSelectSumForEquivalent() {
		ulong result = 0;
		for (ulong i = 0; i < LoopIterations; i++) {
			for (int j = 0; j < RandomValues.Length; j++) {
				result += RandomValues[j] + RandomValues[j] + 1;
			}
		}

		return result;
	}
}