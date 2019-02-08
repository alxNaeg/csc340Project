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
    public partial class lab430 : Form
    {
        //variables for the class
        Boolean Status = false;
        Boolean run1 = false;
        List<Button> buttons = new List<Button>();
        List<string> IDs = new List<string>();
        List<string> status = new List<string>();

        public lab430()
        {
            //update all objects on load
            InitializeComponent();
            ButtonAssign(collectButtons(), MySQLList(), MySQLStatus(), schedule, printer, projector, inClass);
        }
        private List<Button> collectButtons()

        {
            //add all computer buttons to a list, and give them 'single_button_click' code
            List<Button> newButtons = new List<Button>();
            foreach (Control c in this.Controls)
            {
                Button b = c as Button;
                if (b != null && b.Name != "Refresh" && b.Name != "Return")
                {
                    newButtons.Add(b);
                }

            }
            //only add the buttons and the button click code once
            run1 = true;
            //sort the buttons by name
            var newList = newButtons.OrderBy(x => x.Name, new NaturalComparer()).ToList();
            newList.ForEach(Console.WriteLine);
            return newList;

        }

        public void single_button_Click(object sender, EventArgs e)
        {
            //if button is red, update it and sql to green, vice versa
            Button btn = (Button)sender;
            if (btn.BackColor == Color.Red)
            {
                Status = true;
                changeSQLstat1(sender);
                btn.BackColor = Color.Green;


            }
            else if (btn.BackColor == Color.Green)
            {
                Status = false;
                changeSQLstat0(sender);
                btn.BackColor = Color.Red;


            }
        }

        private void changeSQLstat0(object sender)
        {
            //sql code to update a button
            string objname = ((Button)sender).Name;
            int x = Int32.Parse(objname.Substring(6));
            string sql = "UPDATE computer SET status=0 WHERE CR=@Name and room = 430;";
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
            //sql code to update a button
            string objname = ((Button)sender).Name;
            int x = Int32.Parse(objname.Substring(6));
            string sql = "UPDATE computer SET status=1 WHERE  CR=@Name and room = 430;";
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
            //get list of values from sql by computer room id
            string sql = " SELECT CR FROM computer where room = 430;  ";
            MySqlConnection con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            MySqlCommand cmd = new MySqlCommand(sql, con);

            List<string> newIDs = new List<string>();

            try
            {
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    string ID = reader.GetString("CR");
                    newIDs.Add(ID);

                }
                con.Close();
                newIDs.ForEach(Console.WriteLine);
                return newIDs;
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
            newIDs.ForEach(Console.WriteLine);
            return newIDs;
        }

        private List<string> MySQLStatus()
        {
            //get the status of computers in this room
            string sql = " SELECT status FROM computer where room=430  ";
            MySqlConnection con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            MySqlCommand cmd = new MySqlCommand(sql, con);

            List<string> newStatus = new List<string>();

            try
            {
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string stat = reader.GetString("status");
                    newStatus.Add(stat);

                }
                con.Close();
                newStatus.ForEach(Console.WriteLine);
                return newStatus;
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
            newStatus.ForEach(Console.WriteLine);
            return newStatus;
        }

        private static void ButtonAssign(List<Button> buttons, List<string> mysql, List<string> status, Label schedule, Label printer, Label projector, Label inClass)
        {
            //take in 3 lists, lists of buttons, list of status', and list of ids
            
            //update all computers and other values(labels)
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
            //update schedule, lab, and inClass objects

            string sql = "SELECT schedule FROM lab WHERE room = 430;";
            MySqlConnection con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            MySqlCommand cmd = new MySqlCommand(sql, con);
            string sched = "";
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                sched = reader.GetString("schedule");
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
            schedule.Text = sched;

            sql = "SELECT printer FROM lab WHERE room = 430;";
            con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            cmd = new MySqlCommand(sql, con);
            int print = 0;
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                print = Int32.Parse(reader.GetString("printer"));
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
            if (print == 1)
                printer.Text = "Printer: Available";
            else
                printer.Text = "Printer: Unavailable";

            sql = "SELECT projector FROM lab WHERE room = 430;";
            con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            cmd = new MySqlCommand(sql, con);
            int project = 0;
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                project = Int32.Parse(reader.GetString("projector"));
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
            if (project == 1)
                projector.Text = "Projector: Available";
            else
                projector.Text = "Projector: Unavailable";

            inClass.Text = "";

            sql = "SELECT schedule FROM lab WHERE room = 430;";
            con = new MySqlConnection("host=csdatabase.eku.edu;user=naegle;password=NLHFQB;database=naegle;SslMode=none");
            cmd = new MySqlCommand(sql, con);
            sched = "";
            try
            {
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                sched = reader.GetString("schedule");

            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message);
            }
            //parse the schedule from sql, check if this lab has a class or not right now
            int i = 0;
            while (i < sched.Length)
            {
                int j = sched.IndexOf(',', i);
                if (j == -1)
                    j = sched.Length;

                String t1 = sched.Substring(i, 5);
                String t2 = sched.Substring(i + 6, 5);

                int t1h = Int32.Parse(t1.Substring(0, 2));
                int t1m = Int32.Parse(t1.Substring(3, 2));

                int t2h = Int32.Parse(t2.Substring(0, 2));
                int t2m = Int32.Parse(t2.Substring(3, 2));

                TimeSpan start = new TimeSpan(t1h, t1m, 0);
                TimeSpan end = new TimeSpan(t2h, t2m, 0);
                TimeSpan now = DateTime.Now.TimeOfDay;

                if ((now >= start) && (now <= end))
                {
                    inClass.Text = "IN CLASS";
                }

                i = j + 2;

            }

        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            //refresh the page from sql
            ButtonAssign(collectButtons(), MySQLList(), MySQLStatus(), schedule, printer, projector, inClass);
        }

    }
    //natural comparer, sorts things from 'naturally' vs strictly alphabetically
    //ex.
    //aplhabetically - 1,10,11,2,3,4,5,6,7,8,9
    //naturally - 1,2,3,4,5,6,7,8,9,10,11
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
