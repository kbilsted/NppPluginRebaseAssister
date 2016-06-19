using Kbg.NppPluginNET.PluginInfrastructure;

namespace RebaseAssister
{
	class FirstWordOfLineSelector
	{
		public Position SelectFirstWordOfLine(ScintillaGateway scintilla)
		{
			int lineNumber = scintilla.GetCurrentLineNumber();

			var lineContent = scintilla.GetLine(lineNumber);
			if (lineContent.StartsWith("#"))
				return null;

			int endOfFirstWordOnLine = lineContent.IndexOf(' ');
			if (endOfFirstWordOnLine == -1)
				return null;

			var positionOfLine = scintilla.PositionFromLine(lineNumber);
			var cursorpos = scintilla.GetCurrentPos() - positionOfLine;
			var cursorIsWithinFirstWord = cursorpos.Value <= endOfFirstWordOnLine;
			if (!cursorIsWithinFirstWord)
				return null;

			var newPosition = new Position(positionOfLine.Value + endOfFirstWordOnLine);
			scintilla.SetAnchor(newPosition);
			scintilla.SetCurrentPos(positionOfLine);

			return newPosition;
		}
	}
}
