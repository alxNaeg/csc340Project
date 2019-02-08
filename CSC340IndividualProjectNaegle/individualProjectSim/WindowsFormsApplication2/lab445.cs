using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace individualProject
{
    public partial class lab445 : Form
    {
        Boolean Status = false;
        List<Button> buttons = new List<Button>();
        List<string> IDs = new List<string>();
        List<String> status = new List<string>();
        public lab445()
        {
            InitializeComponent();
            ButtonAssign(collectButtons(), MySQLStatus());
        }
        private List<Button> collectButtons()

        {

            foreach (Control c in this.Controls)
            {
                Button b = c as Button;
                if (b != null && b.Name != "Refresh")
                {
                    buttons.Add(b);
                    

                }
            }
            var newList = buttons.OrderBy(x => x.Name, new NaturalComparer()).ToList();
            newList.ForEach(Console.WriteLine);
            return newList;

        }





       /* public void single_button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.BackColor == Color.Red)
            {
                Status = true;
                
                btn.BackColor = Color.Green;


            }
            else if (btn.BackColor == Color.Green)
            {
                Status = false;
                
                btn.BackColor = Color.Red;


            }
        }
        private void changeSQLstat0(object sender)
        {

            string objname = ((Button)sender).Name;
            int x = Int32.Parse(objname.Substring(4));
            string sql = "UPDATE computer SET status=0 WHERE ID=@Name;";
            MySqlConnection con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {

                con.Open();
                cmd.Parameters.Add("@Name", x);
                MySqlDataReader reader = cmd.ExecuteReader();

            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
        }

        private void changeSQLstat1(object sender)
        {
            string objname = ((Button)sender).Name;
            int x = Int32.Parse(objname.Substring(4));
            string sql = "UPDATE computer SET status=1 WHERE ID=@Name;";
            MySqlConnection con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                con.Open();
                cmd.Parameters.Add("@Name", x);
                MySqlDataReader reader = cmd.ExecuteReader();


            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }

        }



        private List<string> MySQLList()
        {

            string sql = " SELECT CR FROM computer where room = 445;  ";
            MySqlConnection con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string ID = reader.GetString("ID");
                    IDs.Add(ID);

                }
                con.Close();
                IDs.ForEach(Console.WriteLine);
                return IDs;
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
            IDs.ForEach(Console.WriteLine);
            return IDs;
        }
        */
        private List<string> MySQLStatus()
        {

            string sql = " SELECT status FROM computer where room=445  ";
            MySqlConnection con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string stat = reader.GetString("status");
                    status.Add(stat);

                }
                con.Close();
                status.ForEach(Console.WriteLine);
                return status;
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
            status.ForEach(Console.WriteLine);
            return status;
        }

        private static void ButtonAssign(List<Button> buttons,  List<string> status)
        {
            int count = 0;
            foreach (Button b in buttons)
            {
                if (status[count].Equals("0"))
                {
                    b.BackColor = Color.Red;
                    count++;
                }
                else
                {
                    b.BackColor = Color.Green;
                    count++;
                }
            }

        }

    }




    public class NaturalComparer : IComparer<string>, IComparer
    {

        private StringParser mParser1;
        private StringParser mParser2;
        private NaturalComparerOptions mNaturalComparerOptions;

        private enum TokenType
        {
            Nothing,
            Numerical,
            String
        }

        private class StringParser
        {
            private TokenType mTokenType;
            private string mStringValue;
            private decimal mNumericalValue;
            private int mIdx;
            private string mSource;
            private int mLen;
            private char mCurChar;
            private NaturalComparer mNaturalComparer;

            public StringParser(NaturalComparer naturalComparer)
            {
                mNaturalComparer = naturalComparer;
            }

            public void Init(string source)
            {
                if (source == null)
                    source = string.Empty;
                mSource = source;
                mLen = source.Length;
                mIdx = -1;
                mNumericalValue = 0;
                NextChar();
                NextToken();
            }

            public TokenType TokenType
            {
                get { return mTokenType; }
            }

            public decimal NumericalValue
            {
                get
                {
                    if (mTokenType == NaturalComparer.TokenType.Numerical)
                    {
                        return mNumericalValue;
                    }
                    else
                    {
                        throw new NaturalComparerException("Internal Error: NumericalValue called on a non numerical value.");
                    }
                }
            }

            public string StringValue
            {
                get { return mStringValue; }
            }

            public void NextToken()
            {
                do
                {
                    //CharUnicodeInfo.GetUnicodeCategory 
                    if (mCurChar == '\0')
                    {
                        mTokenType = NaturalComparer.TokenType.Nothing;
                        mStringValue = null;
                        return;
                    }
                    else if (char.IsDigit(mCurChar))
                    {
                        ParseNumericalValue();
                        return;
                    }
                    else if (char.IsLetter(mCurChar))
                    {
                        ParseString();
                        return;
                    }
                    else
                    {
                        //ignore this character and loop some more 
                        NextChar();
                    }
                }
                while (true);
            }

            private void NextChar()
            {
                mIdx += 1;
                if (mIdx >= mLen)
                {
                    mCurChar = '\0';
                }
                else
                {
                    mCurChar = mSource[mIdx];
                }
            }

            private void ParseNumericalValue()
            {
                int start = mIdx;
                char NumberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
                char NumberGroupSeparator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator[0];
                do
                {
                    NextChar();
                    if (mCurChar == NumberDecimalSeparator)
                    {
                        // parse digits after the Decimal Separator 
                        do
                        {
                            NextChar();
                            if (!char.IsDigit(mCurChar) && mCurChar != NumberGroupSeparator)
                                break;

                        }
                        while (true);
                        break;
                    }
                    else
                    {
                        if (!char.IsDigit(mCurChar) && mCurChar != NumberGroupSeparator)
                            break;
                    }
                }
                while (true);
                mStringValue = mSource.Substring(start, mIdx - start);
                if (decimal.TryParse(mStringValue, out mNumericalValue))
                {
                    mTokenType = NaturalComparer.TokenType.Numerical;
                }
                else
                {
                    // We probably have a too long value 
                    mTokenType = NaturalComparer.TokenType.String;
                }
            }

            private void ParseString()
            {
                int start = mIdx;
                bool roman = (mNaturalComparer.mNaturalComparerOptions & NaturalComparerOptions.RomanNumbers) != 0;
                int romanValue = 0;
                int lastRoman = int.MaxValue;
                int cptLastRoman = 0;
                do
                {
                    if (roman)
                    {
                        int thisRomanValue = RomanLetterValue(mCurChar);
                        if (thisRomanValue > 0)
                        {
                            bool handled = false;

                            if ((thisRomanValue == 1 || thisRomanValue == 10 || thisRomanValue == 100))
                            {
                                NextChar();
                                int nextRomanValue = RomanLetterValue(mCurChar);
                                if (nextRomanValue == thisRomanValue * 10 | nextRomanValue == thisRomanValue * 5)
                                {
                                    handled = true;
                                    if (nextRomanValue <= lastRoman)
                                    {
                                        romanValue += nextRomanValue - thisRomanValue;
                                        NextChar();
                                        lastRoman = thisRomanValue / 10;
                                        cptLastRoman = 0;
                                    }
                                    else
                                    {
                                        roman = false;
                                    }
                                }
                            }
                            else
                            {
                                NextChar();
                            }
                            if (!handled)
                            {
                                if (thisRomanValue <= lastRoman)
                                {
                                    romanValue += thisRomanValue;
                                    if (lastRoman == thisRomanValue)
                                    {
                                        cptLastRoman += 1;
                                        switch (thisRomanValue)
                                        {
                                            case 1:
                                            case 10:
                                            case 100:
                                                if (cptLastRoman > 4)
                                                    roman = false;

                                                break;
                                            case 5:
                                            case 50:
                                            case 500:
                                                if (cptLastRoman > 1)
                                                    roman = false;

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        lastRoman = thisRomanValue;
                                        cptLastRoman = 1;
                                    }
                                }
                                else
                                {
                                    roman = false;
                                }
                            }
                        }
                        else
                        {
                            roman = false;
                        }
                    }
                    else
                    {
                        NextChar();
                    }
                    if (!char.IsLetter(mCurChar)) break;
                }
                while (true);
                mStringValue = mSource.Substring(start, mIdx - start);
                if (roman)
                {
                    mNumericalValue = romanValue;
                    mTokenType = NaturalComparer.TokenType.Numerical;
                }
                else
                {
                    mTokenType = NaturalComparer.TokenType.String;
                }
            }

        }

        public NaturalComparer(NaturalComparerOptions NaturalComparerOptions)
        {
            mNaturalComparerOptions = NaturalComparerOptions;
            mParser1 = new StringParser(this);
            mParser2 = new StringParser(this);
        }

        public NaturalComparer()
           : this(NaturalComparerOptions.Default)
        {
        }

        int System.Collections.Generic.IComparer<string>.Compare(string string1, string string2)
        {
            mParser1.Init(string1);
            mParser2.Init(string2);
            int result;
            do
            {
                if (mParser1.TokenType == TokenType.Numerical & mParser2.TokenType == TokenType.Numerical)
                {
                    // both string1 and string2 are numerical 
                    result = decimal.Compare(mParser1.NumericalValue, mParser2.NumericalValue);
                }
                else
                {
                    result = string.Compare(mParser1.StringValue, mParser2.StringValue);
                }
                if (result != 0)
                {
                    return result;
                }
                else
                {
                    mParser1.NextToken();
                    mParser2.NextToken();
                }
            }
            while (!(mParser1.TokenType == TokenType.Nothing & mParser2.TokenType == TokenType.Nothing));
            //identical 
            return 0;
        }

        private static int RomanLetterValue(char c)
        {
            switch (c)
            {
                case 'I':
                    return 1;
                case 'V':
                    return 5;
                case 'X':
                    return 10;
                case 'L':
                    return 50;
                case 'C':
                    return 100;
                case 'D':
                    return 500;
                case 'M':
                    return 1000;
                default:
                    return 0;
            }
        }

        public int RomanValue(string string1)
        {
            mParser1.Init(string1);

            if (mParser1.TokenType == TokenType.Numerical)
            {
                return (int)mParser1.NumericalValue;
            }
            else
            {
                return 0;
            }
        }

        int IComparer.Compare(object x, object y)
        {
            return ((System.Collections.Generic.IComparer<string>)this).Compare((string)x, (string)y);
        }
    }

    public class NaturalComparerException : System.Exception
    {

        public NaturalComparerException(string msg)
           : base(msg)
        {
        }
    }

    [System.Flags()]
    public enum NaturalComparerOptions
    {
        None,
        RomanNumbers,
        //DecimalValues <- we could put this as an option 
        //IgnoreSpaces <- we could put this as an option 
        //IgnorePunctuation <- we could put this as an option 
        Default = None
    }

}



