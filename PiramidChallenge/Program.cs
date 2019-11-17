
using System;
using System.Collections.Generic;
using System.IO;

namespace PiramidChallenge
{
	public class Program
	{		
		public static List<int> path = new List<int>();
		public static string resultPath;
		public static int resultValue = 0;

		static void Main(string[] args)
		{
			var data = GetDataFromFile();
			Search(data, 0, 0);

			Console.WriteLine("Result Value: " + resultValue);
			Console.WriteLine("Result Path: " + resultPath);

			Console.ReadLine();
		}

		public static bool Search(List<List<Node>> data, int yIndex, int xIndex)
		{
			if (GetNode(data, yIndex, xIndex).IsRoot && GetNode(data, yIndex, xIndex).AddedToPath == false)
			{
				GetNode(data, yIndex, xIndex).AddedToPath = true;
				path.Add(GetNode(data, yIndex, xIndex).Value);
			}

			//var temp = string.Empty;
			//foreach (var value in path)
			//{
			//	temp += value + ">";
			//}
			//Console.WriteLine(temp);

			if (GetNode(data, yIndex, xIndex).IsLeaf)
			{
				string stringPath = string.Empty;
				int result = 0;
				foreach (var value in path)
				{
					result += value;
					stringPath += value + ">";
				}

				//Console.WriteLine(stringPath + " " + result);

				if(result > resultValue)
				{
					resultValue = result;
					resultPath = stringPath;
				}

				path.RemoveAt(path.Count - 1);

				var parentYIndex = GetNode(data, yIndex, xIndex).Parent.yIndex;
				var parentXIndex = GetNode(data, yIndex, xIndex).Parent.xIndex;

				Search(data, parentYIndex, parentXIndex);
			} 
			else if(CanMoveDown(data, yIndex, xIndex))
			{	
				SetParent(GetNode(data, yIndex + 1, xIndex), yIndex, xIndex, true, false);
				path.Add(GetBottomChild(data, yIndex, xIndex).Value);

				yIndex = yIndex + 1;
				Search(data, yIndex, xIndex);
			} 
			else if(CanMoveRight(data, yIndex, xIndex))
			{
				SetParent(GetNode(data, yIndex + 1, xIndex + 1), yIndex, xIndex, false, true);
				path.Add(GetRightChild(data, yIndex, xIndex).Value);

				yIndex = yIndex + 1;
				xIndex = xIndex + 1;
				Search(data, yIndex, xIndex);
			}

			else if (GetNode(data, yIndex, xIndex).IsRoot == false &&
				CanMoveDown(data, yIndex, xIndex) == false &&
				CanMoveRight(data, yIndex, xIndex) == false)
			{
				if (path.Count != 0)
				{
					path.RemoveAt(path.Count - 1);
				}

				GetBottomChild(data, yIndex, xIndex).VisitedByTopParent = false;
				GetRightChild(data, yIndex, xIndex).VisitedByLeftParent = false;

				Search(data, GetNode(data, yIndex, xIndex).Parent.yIndex, GetNode(data, yIndex, xIndex).Parent.xIndex);
			}
			else
			{
				return true;
			}

			return true;
		}

		public static bool CanMoveDown(List<List<Node>> data, int yIndex, int xIndex)
		{
			bool result = false;

			if (GetNode(data, yIndex, xIndex).IsEvent)
			{
				result = GetNode(data, yIndex, xIndex).IsLeaf == false &&
					   GetBottomChild(data, yIndex, xIndex).IsOdd &&
					   GetBottomChild(data, yIndex, xIndex).VisitedByTopParent == false;
			}
			else if (GetNode(data, yIndex, xIndex).IsOdd)
			{
				result =  GetNode(data, yIndex, xIndex).IsLeaf == false &&
					   GetBottomChild(data, yIndex, xIndex).IsEvent &&
					   GetBottomChild(data, yIndex, xIndex).VisitedByTopParent == false;
			}

			return result;
		}

		public static bool CanMoveRight(List<List<Node>> data, int yIndex, int xIndex)
		{
			bool result = false;

			if (GetNode(data, yIndex, xIndex).IsEvent)
			{
				result = GetNode(data, yIndex, xIndex).IsLeaf == false &&
					   GetRightChild(data, yIndex, xIndex).IsOdd &&
					   GetRightChild(data, yIndex, xIndex).VisitedByLeftParent == false;
			}
			else if (GetNode(data, yIndex, xIndex).IsOdd)
			{
				result = GetNode(data, yIndex, xIndex).IsLeaf == false &&
					   GetRightChild(data, yIndex, xIndex).IsEvent &&
					   GetRightChild(data, yIndex, xIndex).VisitedByLeftParent == false;
			}

			return result;
		}

		public static Node GetNode(List<List<Node>> data, int yIndex, int xIndex)
		{
			return data[yIndex][xIndex];
		}

		public static Node GetBottomChild(List<List<Node>> data, int yIndex, int xIndex)
		{
			return data[yIndex + 1][xIndex];
		}

		public static Node GetRightChild(List<List<Node>> data, int yIndex, int xIndex)
		{
			return data[yIndex + 1][xIndex + 1];
		}

		public static void SetParent(Node node, int yIndex, int xIndex, bool visitedByTopNode, bool visistedByLeftNode)
		{
			node.Parent.yIndex = yIndex;
			node.Parent.xIndex = xIndex;
			node.VisitedByTopParent = visitedByTopNode;
			node.VisitedByLeftParent = visistedByLeftNode;
		}
		
		public static List<List<Node>> GetDataFromFile()
		{
			var data = new List<List<Node>>();

			string[] lines = File.ReadAllLines(@"..\..\Data.txt");

			for(int i = 0; i < lines.Length; i++)
			{
				var row = new List<Node>();
				var values = lines[i].Split(' ');
				for(int j = 0; j < values.Length; j++)
				{
					var node = new Node
					{
						Value = int.Parse(values[j]),
						IsLeaf = (i + 1) == lines.Length,
						IsEvent = int.Parse(values[j]) % 2 == 0,
						IsOdd = int.Parse(values[j]) % 2 != 0,
						IsRoot = i == 0,
						Parent = new Parent()
					};
					row.Add(node);
				}
				data.Add(row);
			}

			return data;
		}

		public static List<List<Node>> GetTestData()
		{
			var data = new List<List<Node>>();

			var row0 = new List<Node>();
			var node0 = new Node
			{
				Value = 1,
				IsLeaf = false,
				IsEvent = false,
				IsOdd = true,
				IsRoot = true,
				Parent = new Parent()
			};
			row0.Add(node0);
			data.Add(row0);

			var row1 = new List<Node>();
			var node10 = new Node
			{
				Value = 8,
				IsLeaf = false,
				IsEvent = true,
				IsOdd = false,
				Parent = new Parent()
			};
			row1.Add(node10);
			var node11 = new Node
			{
				Value = 9,
				IsLeaf = false,
				IsEvent = true,
				IsOdd = false,
				Parent = new Parent()
			};
			row1.Add(node11);
			data.Add(row1);

			var row2 = new List<Node>();
			var node20 = new Node
			{
				Value = 1,
				IsLeaf = false,
				IsEvent = false,
				IsOdd = true,
				Parent = new Parent()
			};
			row2.Add(node20);
			var node21 = new Node
			{
				Value = 5,
				IsLeaf = false,
				IsEvent = false,
				IsOdd = true,
				Parent = new Parent()
			};
			row2.Add(node21);
			var node23 = new Node
			{
				Value = 9,
				IsLeaf = false,
				IsEvent = true,
				IsOdd = false,
				Parent = new Parent()
			};
			row2.Add(node23);
			data.Add(row2);
			
			var row3 = new List<Node>();
			var node30 = new Node
			{
				Value = 4,
				IsLeaf = true,
				IsEvent = true,
				IsOdd = false,
				Parent = new Parent()
			};
			row3.Add(node30);

			var node31 = new Node
			{
				Value = 5,
				IsLeaf = true,
				IsEvent = false,
				IsOdd = true,
				Parent = new Parent()
			};
			row3.Add(node31);

			var node32 = new Node
			{
				Value = 2,
				IsLeaf = true,
				IsEvent = true,
				IsOdd = false,
				Parent = new Parent()
			};
			row3.Add(node32);

			var node33 = new Node
			{
				Value = 3,
				IsLeaf = true,
				IsEvent = false,
				IsOdd = true,
				Parent = new Parent()
			};
			row3.Add(node33);
			data.Add(row3);

			return data;
		}

	}

	public class Node
	{
		public int Value { get; set; }

		public bool VisitedByTopParent { get; set; } = false;

		public bool VisitedByLeftParent { get; set; } = false;

		public bool IsLeaf { get; set; }

		public Parent Parent { get; set; }

		public bool IsEvent { get; set; }

		public bool IsOdd { get; set; }

		public bool IsRoot { get; set; } = false;

		public bool AddedToPath { get; set; } = false;
	}

	public class Parent
	{
		public int xIndex { get; set; } = 0; 

		public int yIndex { get; set; } = 0;
	}          

}

 





