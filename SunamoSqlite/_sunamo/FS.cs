

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoSqlite._sunamo;

internal class FS
{
    internal static string Combine(string nad, string nazevSpustitelneExeDB)
    {
        return Path.Combine(nad, nazevSpustitelneExeDB);
    }

    internal static void CopyTo(string v, string nad, FileMoveCollisionOption overwrite)
    {
        File.Copy(v, Path.Combine(nad, Path.GetFileName(v)), overwrite == FileMoveCollisionOption.Overwrite);
    }
}
