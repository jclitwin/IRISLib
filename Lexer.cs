using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IRISLib.Token;

namespace IRISLib
{
    public class Lexer
    {
        /// Lexer state
        public enum eLEXER
        {
            eIN_NULL = 0,
            eIN_START,
            eIN_DONE,
            eIN_NUM,
            eIN_FLOAT,
            eIN_VAR,
            eIN_STRING,
            eIN_MINUS,
            eIN_DIV,
            eIN_COMMENT
        };

        /// Lexer table (store the start state and token type of the first character)
        struct sLexTable
        {
            public int mType;
            public int mState;
        };

        static readonly sLexTable[] gLexTable = new[]
        {
            new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL },    /// 0x00
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x01
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x02
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x03
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x04
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x05
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x06
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x07
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x08
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x09
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x0a
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x0b
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x0c
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x0d
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x0e
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x0f
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x10
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x11
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x12
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x13
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x14
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x15
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x16
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x17
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x18
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x19
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x1a
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x1b
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x1c
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x1d
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x1e
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// 0x1f
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// ' '
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NOT, mState = (int)eLEXER.eIN_NULL},    /// '!'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_STRING},  /// '"'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// '#'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_DOLLAR, mState = (int)eLEXER.eIN_NULL},    /// '$'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_PERCENT, mState = (int)eLEXER.eIN_NULL},    /// '%'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_AND, mState = (int)eLEXER.eIN_NULL},    /// '&'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_STRING},  /// '\''
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_LPAREN, mState = (int)eLEXER.eIN_NULL},    /// '( '
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_RPAREN, mState = (int)eLEXER.eIN_NULL},    /// ' )'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_MUL, mState = (int)eLEXER.eIN_NULL},    /// '*'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_PLUS, mState = (int)eLEXER.eIN_NULL},    /// '+'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_COMMA, mState = (int)eLEXER.eIN_NULL},    /// ','
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_MINUS},   /// '-'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_DOT, mState = (int)eLEXER.eIN_NULL},    /// '.'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_DIV},     /// '/'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '0'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '1'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '2'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '3'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '4'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '5'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '6'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '7'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '8'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NUM},     /// '9'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_COLON, mState = (int)eLEXER.eIN_NULL},    /// ':'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_SEMICOLON, mState = (int)eLEXER.eIN_NULL},    /// ';'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_LANGLE, mState = (int)eLEXER.eIN_NULL},    /// '<'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_ASSIGN, mState = (int)eLEXER.eIN_NULL},    /// '='
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_RANGLE, mState = (int)eLEXER.eIN_NULL},    /// '>'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_QUESTION, mState = (int)eLEXER.eIN_NULL},    /// '?'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_AT, mState = (int)eLEXER.eIN_NULL},    /// '@'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'A'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'B'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'C'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'D'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'E'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'F'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'G'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'H'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'I'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'J'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'K'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'L'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'M'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'N'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'O'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'P'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'Q'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'R'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'S'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'T'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'U'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'V'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'W'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'X'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'Y'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'Z'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_LSQUARE, mState = (int)eLEXER.eIN_NULL},    /// '['
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// '\\'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_RSQUARE, mState = (int)eLEXER.eIN_NULL},    /// ']'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_XOR, mState = (int)eLEXER.eIN_NULL},    /// '^'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// '_'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL},    /// '`'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'a'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'b'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'c'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'd'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'e'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'f'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'g'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'h'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'i'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'j'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'k'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'l'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'm'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'n'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'o'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'p'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'q'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'r'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 's'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 't'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'uint'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'v'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'w'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'x'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'y'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_VAR},     /// 'z'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_LCURLY, mState = (int)eLEXER.eIN_NULL},    /// '{'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_OR, mState = (int)eLEXER.eIN_NULL},    /// '|'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_RCURLY, mState = (int)eLEXER.eIN_NULL},    /// '}'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_TILD, mState = (int)eLEXER.eIN_NULL},    /// '~'
        	new sLexTable { mType = (int)eTOKEN.eTOKEN_NULL, mState = (int)eLEXER.eIN_NULL}     /// 0x7f
        };

        static int GetLexTableSize()
        {
            return System.Runtime.InteropServices.Marshal.SizeOf(gLexTable) / System.Runtime.InteropServices.Marshal.SizeOf(typeof(sLexTable));
        }

        // Variables
        protected byte[] _buffer;
        protected uint _bufferLen = 0;
        protected uint _pos = 0;
        protected int _line = 0;

        protected Dictionary<string, int> _keywordMap = new Dictionary<string, int>();
        // End variables.

        public void SetSource(byte[] buffer, uint size)
        {
            _buffer = buffer;
            _bufferLen = size;
            _pos = 0;
            _line = 0;
        }


        public bool IsEnd()
        {
            while (_pos < _bufferLen)
            {
                byte c = _buffer[_pos];
                if (c == '\r')
                {
                    _pos++;
                }
                else if (c == ' ')
                {
                    _pos++;
                }
                else if (c == '\n')
                {
                    _pos++;
                    _line++;
                }
                else if (c == '	')
                {
                    _pos++;
                }
                else if (c == '\xEF')
                {
                    _pos++;
                }
                else if (c == '\xBB')
                {
                    _pos++;
                }
                else if (c == '\xBF')
                {
                    _pos++;
                }
                else
                {
                    break;
                }
            }

            return (_pos >= _bufferLen);
        }

        public byte GetNextChar()
        {
            if (_pos < _bufferLen)
            {
                byte c = _buffer[_pos];
                if(c == '\n')
                {
                    ++_line;
                }

                ++_pos;
                return c;
            }

            return 0;
        }

        public void UngetNextChar()
        {
            --_pos;
            byte c = _buffer[_pos];
            if(c == '\n')
            {
                --_line;
            }
        }
    }
}
