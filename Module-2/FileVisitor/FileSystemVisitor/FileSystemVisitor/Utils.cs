using FileSystemVisitor.Models;

namespace FileSystemVisitor
{
	public static class Utils
	{
		public static string CalcShiftLevel(IChildNode node, string shift = "")
		{
			if (node.Parent == null)
			{
				return shift;
			}

			return CalcShiftLevel(node.Parent, shift + '\t');
		}

	}
}
