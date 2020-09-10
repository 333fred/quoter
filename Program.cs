using System;
using System.Text;

const int LineLength = 20;
const int MaxLines = 8;

var builder = new StringBuilder();
int curIndex = 0;
var input = args[0];
var numLines = 0;

builder.Append("write_quote(PSTR(");

do
{
    // Look LineLength characters past the current index.
    // If that character is past the end of the string, the
    // current subexpression is curIndex..end. If it's a space,
    // then it's curIndex..curIndex+20. Otherwise, look back until
    // a space is found, and it's curIndex..that found index. If
    // there is no previous space, error

    var i20 = curIndex + LineLength;
    var lastIndex = i20 >= input.Length
        ? input.Length
        : input[i20] == ' ' ? i20 : findPreviousIndexOfSpace();

    var lengthOfInput = lastIndex - curIndex;
    var leadPadding = (int)Math.Floor(((float)LineLength - lengthOfInput) / 2);
    var trailingPadding = (int)Math.Ceiling(((float)LineLength - lengthOfInput) / 2);

    if (++numLines > MaxLines)
    {
        throw new InvalidOperationException($"Input is spread out to too many lines!\n{builder.ToString()}");
    }

    builder.Append($@"""{new string(' ', leadPadding)}{input[curIndex..lastIndex]}");
    if (lastIndex != input.Length)
    {
        builder.AppendLine($@"{new string(' ', trailingPadding)}\n""");
    }
    else
    {
        builder.AppendLine(@$"""), {numLines}); break;");
    }

    curIndex = lastIndex + 1;

    int findPreviousIndexOfSpace()
    {
        for (int i = i20; i > curIndex; i--)
        {
            if (input[i] == ' ') return i;
        }

        throw new InvalidOperationException($"Could not find a space in the 20 characters after {curIndex}\n{builder.ToString()}");
    }
} while (curIndex < input.Length);

Console.WriteLine(builder.ToString());
