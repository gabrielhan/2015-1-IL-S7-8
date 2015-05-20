using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class StringTokenizer : ITokenizer
    {
        string _toParse;
        int _pos;
        int _maxPos;
        TokenType _curToken;
        double _doubleValue;

        public StringTokenizer( string s )
            : this( s, 0, s.Length )
        {
        }

        public StringTokenizer( string s, int startIndex )
            : this( s, startIndex, s.Length )
        {
        }

        public StringTokenizer( string s, int startIndex, int count )
        {
            _curToken = TokenType.None;
            _toParse = s;
            _pos = startIndex;
            _maxPos = startIndex + count;
        }

        #region Input reader

        char Peek()
        {
            Debug.Assert( !IsEnd );
            return _toParse[_pos];
        }

        char Read()
        {
            Debug.Assert( !IsEnd );
            return _toParse[_pos++];
        }

        void Forward()
        {
            Debug.Assert( !IsEnd );
            ++_pos;
        }

        bool IsEnd
        {
            get { return _pos >= _maxPos; }
        }

        #endregion

        public TokenType CurrentToken
        {
            get { return _curToken; }
        }

        public bool Match( TokenType t )
        {
            if( _curToken == t )
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchDouble( out double value )
        {
            value = _doubleValue;
            if( _curToken == TokenType.Number )
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger( int expectedValue )
        {
            if( _curToken == TokenType.Number
                && _doubleValue < Int32.MaxValue
                && (int)_doubleValue == expectedValue )
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger( out int value )
        {
            if( _curToken == TokenType.Number
                && _doubleValue < Int32.MaxValue )
            {
                value = (int)_doubleValue;
                GetNextToken();
                return true;
            }
            value = 0;
            return false;
        }

        public TokenType GetNextToken()
        {
            if( IsEnd ) return _curToken = TokenType.EndOfInput;
            char c = Read();
            while( Char.IsWhiteSpace( c ) )
            {
                if( IsEnd ) return _curToken = TokenType.EndOfInput;
                c = Read();
            }
            switch( c )
            {
                case '+': _curToken = TokenType.Plus; break;
                case '-': _curToken = TokenType.Minus; break;
                case '*': _curToken = TokenType.Mult; break;
                case '/': _curToken = TokenType.Div; break;
                case '(': _curToken = TokenType.OpenPar; break;
                case ')': _curToken = TokenType.ClosePar; break;
                default:
                    {
                        if( Char.IsDigit( c ) )
                        {
                            _curToken = TokenType.Number;
                            double val = (int)(c - '0');
                            while( !IsEnd && Char.IsDigit( c = Peek() ) )
                            {
                                val = val * 10 + (int)(c - '0');
                                Forward();
                            }
                            _doubleValue = val;
                        }
                        else _curToken = TokenType.Error;
                        break;
                    }
            }
            return _curToken;
        }

    }
}



namespace gaby.Parser
{
    public class StringTokenizer : ITokenizer
    {
        string _toParse;
        int _pos;
        int _maxPos;
        TokenType _curToken;
        double _doubleValue;
        string _identifier;

        public StringTokenizer(string s)
            : this(s, 0, s.Length)
        {
        }

        public StringTokenizer(string s, int startIndex)
            : this(s, startIndex, s.Length)
        {
        }

        public StringTokenizer(string s, int startIndex, int count)
        {
            _curToken = TokenType.None;
            _toParse = s;
            _pos = startIndex;
            _maxPos = startIndex + count;
        }

        #region Input reader

        char Peek()
        {
            Debug.Assert(!IsEnd);
            return _toParse[_pos];
        }

        char Read()
        {
            Debug.Assert(!IsEnd);
            return _toParse[_pos++];
        }

        void Forward()
        {
            Debug.Assert(!IsEnd);
            ++_pos;
        }

        bool IsEnd
        {
            get { return _pos >= _maxPos; }
        }

        #endregion

        public TokenType CurrentToken
        {
            get { return _curToken; }
        }

        public bool Match(TokenType t)
        {
            if (_curToken == t)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchDouble(out double value)
        {
            value = _doubleValue;
            if (_curToken == TokenType.Number)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger(int value)
        {
            if (_curToken == TokenType.Number
                && _doubleValue < Int32.MaxValue
                && (int)_doubleValue == value)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger(out int value)
        {
            if (_curToken == TokenType.Number
                && _doubleValue < Int32.MaxValue)
            {
                value = (int)_doubleValue;
                GetNextToken();
                return true;
            }
            value = 0;
            return false;
        }


        public bool MatchIdentifier(string value)
        {
            if (_curToken == TokenType.Identifier)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchIdentifier(out string value)
        {
            if (_curToken == TokenType.Identifier)
            {
                value = _identifier;
                GetNextToken();
                return true;
            }
            value = null;
            return false;
        }

        public TokenType GetNextToken()
        {
            char c;
            int seq = 0;
            for (; ; )
            {
                if (IsEnd) return _curToken = TokenType.EndOfInput;
                c = Read();
                if (Char.IsWhiteSpace(c))
                {

                }
                else if (c == '/' && seq == 0)
                {
                    if (Peek() == '/') { Forward(); seq = 1; }
                    else if (Peek() == '*') { Forward(); seq = 2; }
                    else { return _curToken = TokenType.Div; }
                }
                else if (c == '\n' && seq == 1) { seq = 0; }
                else if (c == '*' && seq == 2 && Peek() == '/') { Forward(); seq = 0; }
                else { break; }
            }
            switch (c)
            {
                case '+': _curToken = TokenType.Plus; break;
                case '-': _curToken = TokenType.Minus; break;
                case '*': _curToken = TokenType.Mult; break;
                case '(': _curToken = TokenType.OpenPar; break;
                case ')': _curToken = TokenType.ClosePar; break;
                case ';': _curToken = TokenType.SemiColon; break;
                case '[': _curToken = TokenType.OpenSquare; break;
                case ']': _curToken = TokenType.CloseSquare; break;
                case '{': _curToken = TokenType.OpenBracket; break;
                case '}': _curToken = TokenType.CloseBracket; break;
                case '.': _curToken = TokenType.Dot; break;
                case ',': _curToken = TokenType.Coma; break;
                case '?': _curToken = TokenType.QuestionMark; break;
                case ':':
                    {
                        _curToken = TokenType.Colon;
                        if (Peek() == ':')
                        {
                            _curToken = TokenType.DoubleColon;
                            Forward();
                        }
                        break;
                    }
                default:
                    {

                        if (Char.IsDigit(c))
                        {
                            _curToken = TokenType.Number;
                            double val = (int)(c - '0');
                            while (!IsEnd && Char.IsDigit(c = Peek()))
                            {
                                val = val * 10 + (int)(c - '0');
                                Forward();
                            }
                            _doubleValue = val;
                        }
                        else if (Char.IsLetter(c) || c == '_')
                        {
                            _curToken = TokenType.Identifier;
                            StringBuilder stb = new StringBuilder();
                            stb.Append(c);
                            for (; ; )
                            {
                                if (Char.IsLetterOrDigit(Peek()) || Peek() == '_')
                                {
                                    stb.Append(Peek());
                                    Forward();
                                }
                                else break;
                            }
                            _identifier = stb.ToString();
                        }
                        else
                        {
                            _curToken = TokenType.Error;
                        }
                        break;
                    }
            }
            return _curToken;
        }

    }
}

