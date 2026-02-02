using System;

namespace Psxbox.Utils.Helpers;

public static class BooleanHelpers
{
    public static string ToString(this bool value, string trueString, string falseString)
    {
        return value ? trueString : falseString;
    }
}
