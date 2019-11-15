
using System;
using System.Collections.Generic;

namespace PiramidChallenge
{
	public class Program
	{		
		public static List<int> path = new List<int>();
		public static string resultPath;
		public static int resultValue = 0;

		static void Main(string[] args)
		{
			var data = GetData();
			Search(data, 0, 0);

			Console.WriteLine("Result Value: " + resultValue);
			Console.WriteLine("Result Path: " + resultPath);

			Console.ReadLine();
		}

		public static bool Search(List<List<Node>> data, int yIndex, int xIndex)
		{
			if(GetNode(data, yIndex, xIndex).IsLeaf)
			{
				path.Add(GetNode(data, yIndex, xIndex).Value);

				string stringPath = string.Empty;
				int result = 0;
				foreach (var value in path)
				{
					result += value;
					stringPath += value + ">";
				}
				
				if(result > resultValue)
				{
					resultValue = result;
					resultPath = stringPath;
				}

				path.RemoveAt(path.Count - 1);

				Search(data, GetNode(data, yIndex, xIndex).Parent.yIndex, GetNode(data, yIndex, xIndex).Parent.xIndex);
			}
			
			if(CanMoveDown(data, yIndex, xIndex))
			{	
				SetParent(GetNode(data, yIndex + 1, xIndex), yIndex, xIndex, true, false);
				if(GetNode(data, yIndex, xIndex).AddedToPath == false)
				{
					GetNode(data, yIndex, xIndex).AddedToPath = true;
					path.Add(GetNode(data, yIndex, xIndex).Value);
				}				

				yIndex++;
				Search(data, yIndex, xIndex);
			}

			if(CanMoveRight(data, yIndex, xIndex))
			{
				SetParent(GetNode(data, yIndex + 1, xIndex + 1), yIndex, xIndex, false, true);
				if (GetNode(data, yIndex, xIndex).AddedToPath == false)
				{
					GetNode(data, yIndex, xIndex).AddedToPath = true;
					path.Add(GetNode(data, yIndex, xIndex).Value);
				}

				yIndex++;
				xIndex++;
				Search(data, yIndex, xIndex);
			}

			if (GetNode(data, yIndex, xIndex).IsRoot == false &&
				CanMoveDown(data, yIndex, xIndex) == false &&
				CanMoveRight(data, yIndex, xIndex) == false)
			{		
				if(path.Count != 1)
				{
					path.RemoveAt(path.Count - 1);
				}
				
				Search(data, GetNode(data, yIndex, xIndex).Parent.yIndex, GetNode(data, yIndex, xIndex).Parent.xIndex);
			}

			return true;
		}

		public static bool CanMoveDown(List<List<Node>> data, int yIndex, int xIndex)
		{	
			//assume data node is not leaf
			if(GetNode(data, yIndex, xIndex).IsEvent)
			{
				return GetNode(data, yIndex, xIndex).IsLeaf == false &&
					   GetBottomChild(data, yIndex, xIndex).IsOdd &&
					   GetBottomChild(data, yIndex, xIndex).VisitedByTopParent == false;
			}
			else if (GetNode(data, yIndex, xIndex).IsOdd)
			{
				return GetNode(data, yIndex, xIndex).IsLeaf == false &&
					   GetBottomChild(data, yIndex, xIndex).IsEvent &&
					   GetBottomChild(data, yIndex, xIndex).VisitedByTopParent == false;
			}
			else
			{
				return false;
			}				   
		}

		public static bool CanMoveRight(List<List<Node>> data, int yIndex, int xIndex)
		{
			if (GetNode(data, yIndex, xIndex).IsEvent)
			{
				return GetNode(data, yIndex, xIndex).IsLeaf == false &&
					   GetRightChild(data, yIndex, xIndex).IsOdd &&
					   GetRightChild(data, yIndex, xIndex).VisitedByLeftParent == false;
			}
			else if (GetNode(data, yIndex, xIndex).IsOdd)
			{
				return GetNode(data, yIndex, xIndex).IsLeaf == false &&
						GetRightChild(data, yIndex, xIndex).IsEvent &&
					   GetRightChild(data, yIndex, xIndex).VisitedByLeftParent == false;
			}
			else
			{
				return false;
			}
		}

		public static Node GetNode(List<List<Node>> data, int yIndex, int xIndex)
		{
			return data[yIndex][xIndex];
		}

		public static Node GetBottomChild(List<List<Node>> data, int yIndex, int xIndex)
		{
			yIndex++;
			
			return data[yIndex][xIndex];
		}

		public static Node GetRightChild(List<List<Node>> data, int yIndex, int xIndex)
		{
			yIndex++;
			xIndex++;
			return data[yIndex][xIndex];
		}

		public static void SetParent(Node node, int yIndex, int xIndex, bool visitedByTopNode, bool visistedByLeftNode)
		{
			node.Parent.yIndex = yIndex;
			node.Parent.xIndex = xIndex;
			node.VisitedByTopParent = visitedByTopNode;
			node.VisitedByLeftParent = visistedByLeftNode;
		}

		public static List<List<Node>> GetData()
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
		public int xIndex { get; set; } = 0; //x = position of parent on the x-axis: stores -1 or 0 unless node index in array is 0 then 0

		public int yIndex { get; set; } = 0;  // y = position of parent on the y-axis: current position - 1
	}          
}

 





