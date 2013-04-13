using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SQLFormatter
{
    static class SQLFormatter{

        //public static readonly String[] SQL_KEYWORDS = new string[] { "SELECT", "FROM" };
        public static readonly String[] SQL_KEYWORDS_TO_APPEND_WITH_CR = new string[] { "," };
        public static readonly String[] SQL_KEYWORDS_TO_PREPEND_WITH_CR = new string[] { "SELECT", "INSERT", "UPDATE", "FROM", "ORDER", "GROUP", "LEFT", "RIGHT", "AND", "OR", "WHERE", "CASE", "WHEN", "END", "INNER"};
        public static readonly String[] SQL_KEYWORDS_TO_PREPEND_WITH_TAB = new string[] { "LEFT", "RIGHT", "INNER", "AND", "OR", "WHEN" };
        public static readonly String[] SQL_KEYWORDS_TO_APPEND_WITH_TAB = new string[] {  };

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
         
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }


        public static String formatSQL(string sql)
        {
            return (formatSQL(sql, false));
        }

        public static String formatSQL(string sql, bool singleLine)
        {
            if (String.IsNullOrEmpty(sql)) return String.Empty;
            else
            {
                sql = removeWhitespace(sql);
                sql = formatSQLKeywords(sql);
                if(!singleLine)sql = addSQLWhitespace(sql);
                return sql;
            }
        }

        public static String removeWhitespace(string sql)
        {
            sql = sql.Trim();
            sql = sql.Replace("\r", " ");
            sql = sql.Replace("\n", " ");
            sql = sql.Replace("\t", " ");
            while (sql.Contains("  "))
            {
                sql = sql.Replace("  ", " ");
            }

            sql = sql.Replace(" ,", ",");
            sql = sql.Replace(" ;", ";");
            sql = sql.Replace(" .", ".");

            return sql;
        }

        public static String formatSQLKeywords(string sql)
        {
            //Visual Studio Detritus
            if(sql.StartsWith("\"")) sql = sql.Remove(0, 1);
            sql = Regex.Replace(sql, @"\b\s*""\s*\{.*\}", "");

            //Main
            sql = Regex.Replace(sql, @"\b[Ff][Rr][Oo][Mm]\b", "FROM");
            sql = Regex.Replace(sql, @"\b[Gg][Rr][Oo][Uu][Pp]\b", "GROUP");
            sql = Regex.Replace(sql, @"\b[Ss][Ee][Ll][Ee][Cc][Tt]\b", "SELECT");
            sql = Regex.Replace(sql, @"\b[Ii][Ss][Ee][Rr][Tt]\b", "INSERT");
            sql = Regex.Replace(sql, @"\b[Uu][Pp][Dd][Aa][Tt][Ee]\b", "UPDATE");
            sql = Regex.Replace(sql, @"\b[Ww][Hh][Ee][Rr][Ee]\b", "WHERE");

            //Logic
            sql = Regex.Replace(sql, @"\b[Oo][Rr]\b", "OR");
            sql = Regex.Replace(sql, @"\b[Aa][Nn][Dd]\b", "AND");
            sql = Regex.Replace(sql, @"\b[Cc][Aa][Ss][Ee]\b", "CASE");
            sql = Regex.Replace(sql, @"\b[Ww][Hh][Ee][Nn]\b", "WHEN");
            sql = Regex.Replace(sql, @"\b[Ee][Nn][Dd][Nn]\b", "END");

            //Ordering
            sql = Regex.Replace(sql, @"\b[Oo][Rr][Dd][Ee][Rr]\b", "ORDER");
            sql = Regex.Replace(sql, @"\b[Bb][Yy]\b", "BY");
            sql = Regex.Replace(sql, @"\b[Dd][Ee][Ss][Cc]\b", "DESC");
            sql = Regex.Replace(sql, @"\b[Aa][Ss][Cc]\b", "ASC");
            sql = Regex.Replace(sql, @"\b[Aa][Ss][Cc][Ee][Nn][Dd][Ii][Nn][Gg]\b", "ASCENDING");
            sql = Regex.Replace(sql, @"\b[Aa][Ss][Cc][Ee][Nn][Dd][Ii][Nn][Gg]\b", "DESCENDING");

            //Joins
            sql = Regex.Replace(sql, @"\b[Oo][Uu][Tt][Ee][Rr]\b", "OUTER");
            sql = Regex.Replace(sql, @"\b[Ii][Nn][NN][Ee][Rr]\b", "INNER");
            sql = Regex.Replace(sql, @"\b[Ll][Ee][Ff][Tt]\b", "LEFT");
            sql = Regex.Replace(sql, @"\b[Rr][Ii][Gg][Hh][Tt]\b", "RIGHT");
            sql = Regex.Replace(sql, @"\b[Cc][Rr][Oo][Ss][Ss]\b", "CROSS");
            sql = Regex.Replace(sql, @"\b[Ss][Ee][Ll][Ff]\b", "SELF");
            sql = Regex.Replace(sql, @"\b[Jj][Oo][Ii][Nn]\b", "JOIN");

            //Control Characters
            sql = Regex.Replace(sql, @"""", "'");
            return sql;
        }

        public static String addSQLWhitespace(string sql)
        {
            foreach (String keyword in SQL_KEYWORDS_TO_PREPEND_WITH_CR)
            {
                sql = prependKeywordWithCarraigeReturn(sql, keyword);
            }
            foreach (String keyword in SQL_KEYWORDS_TO_APPEND_WITH_CR)
            {
                sql = appendKeywordWithCarraigeReturn(sql, keyword);
            }

            foreach (String keyword in SQL_KEYWORDS_TO_PREPEND_WITH_TAB)
            {
                sql = prependKeywordWithTab(sql, keyword);
            }
            foreach (String keyword in SQL_KEYWORDS_TO_APPEND_WITH_TAB)
            {
                sql = appendKeywordWithTab(sql, keyword);
            }

            sql = sql.Trim();
            
            return sql;
        }

        public static String prependKeywordWithCarraigeReturn(string source, string keyword)
        {
            return source.Replace(keyword, "\r\n" + keyword);
        }

        public static String appendKeywordWithCarraigeReturn(string source, string keyword)
        {
            return source.Replace(keyword, keyword + "\r\n");
        }

        public static String prependKeywordWithTab(string source, string keyword)
        {
            return Regex.Replace(source, @"\b" + keyword + @"\b", "\t" + keyword);
        }
        
        public static String appendKeywordWithTab(string source, string keyword)
        {
            return Regex.Replace(source, @"\b"+ keyword + @"\b", keyword + "\t");
        }


    }
}
