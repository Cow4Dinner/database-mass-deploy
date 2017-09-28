using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScintillaNET;

namespace MultipleDatabaseManager
{
    static class TextEditor
    {
        public static void EnableJsonHighlighting(ScintillaNET.Scintilla scintilla)
        {
            // Configure the JSON lexer styles
            scintilla.Styles[Style.Json.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Json.BlockComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Json.LineComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Json.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Json.PropertyName].ForeColor = Color.Blue;
            scintilla.Styles[Style.Json.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Json.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Json.Operator].ForeColor = Color.Purple;
            scintilla.Lexer = Lexer.Json;
        }

        public static void EnableSqlHighlighting(ScintillaNET.Scintilla scintilla)
        {
                scintilla.WrapMode = WrapMode.Word;

                // Reset the styles
                scintilla.StyleResetDefault();
                scintilla.Styles[ScintillaNET.Style.Default].Font = "Courier New";
                scintilla.Styles[ScintillaNET.Style.Default].Size = 10;
                scintilla.StyleClearAll();

                // Set the SQL Lexer
                scintilla.Lexer = Lexer.Sql;

                // Show line numbers
                scintilla.Margins[0].Width = 20;

                // Set the Styles
                scintilla.Styles[ScintillaNET.Style.LineNumber].ForeColor = Color.FromArgb(255, 128, 128, 128);  //Dark Gray
                scintilla.Styles[ScintillaNET.Style.LineNumber].BackColor = Color.FromArgb(255, 228, 228, 228);  //Light Gray
                scintilla.Styles[ScintillaNET.Style.Sql.Comment].ForeColor = Color.Green;
                scintilla.Styles[ScintillaNET.Style.Sql.CommentLine].ForeColor = Color.Green;
                scintilla.Styles[ScintillaNET.Style.Sql.CommentLineDoc].ForeColor = Color.Green;
                scintilla.Styles[ScintillaNET.Style.Sql.Number].ForeColor = Color.Maroon;
                scintilla.Styles[ScintillaNET.Style.Sql.Word].ForeColor = Color.Blue;
                scintilla.Styles[ScintillaNET.Style.Sql.Word2].ForeColor = Color.Fuchsia;
                scintilla.Styles[ScintillaNET.Style.Sql.User1].ForeColor = Color.Gray;
                scintilla.Styles[ScintillaNET.Style.Sql.User2].ForeColor = Color.FromArgb(255, 00, 128, 192);    //Medium Blue-Green
                scintilla.Styles[ScintillaNET.Style.Sql.String].ForeColor = Color.Red;
                scintilla.Styles[ScintillaNET.Style.Sql.Character].ForeColor = Color.Red;
                scintilla.Styles[ScintillaNET.Style.Sql.Operator].ForeColor = Color.Black;

                // Set keyword lists
                // Word = 0
                scintilla.SetKeywords(0, @"add alter as authorization backup begin bigint binary bit break browse bulk by cascade case catch check checkpoint close clustered column commit compute constraint containstable continue create current cursor cursor database date datetime datetime2 datetimeoffset dbcc deallocate decimal declare default delete deny desc disk distinct distributed double drop dump else end errlvl escape except exec execute exit external fetch file fillfactor float for foreign freetext freetexttable from full function goto grant group having hierarchyid holdlock identity identity_insert identitycol if image index insert int intersect into key kill lineno load merge money national nchar nocheck nocount nolock nonclustered ntext numeric nvarchar of off offsets on open opendatasource openquery openrowset openxml option order over percent plan precision primary print proc procedure public raiserror read readtext real reconfigure references replication restore restrict return revert revoke rollback rowcount rowguidcol rule save schema securityaudit select set setuser shutdown smalldatetime smallint smallmoney sql_variant statistics table table tablesample text textsize then time timestamp tinyint to top tran transaction trigger truncate try union unique uniqueidentifier update updatetext use user values varbinary varchar varying view waitfor when where while with writetext xml go ");
                // Word2 = 1
                scintilla.SetKeywords(1, @"ascii cast char charindex ceiling coalesce collate contains convert current_date current_time current_timestamp current_user floor isnull max min nullif object_id session_user substring system_user tsequal ");
                // User1 = 4
                scintilla.SetKeywords(4, @"all and any between cross exists in inner is join left like not null or outer pivot right some unpivot ( ) * ");
                // User2 = 5
                scintilla.SetKeywords(5, @"sys objects sysobjects ");

        }
    }

}
