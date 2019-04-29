using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Lexer
{
    protected byte[] _buffer;
    protected char[] _bufferUnicode;

    protected uint _bufferLen;

    protected uint _pos;

    protected int _line;

    protected Dictionary<string, int> _keywordMap = new Dictionary<string, int>();

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
        //int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(sLexTable));
        //return gLexTable.Length * size / size;
        return 128;
    }

    public Lexer()
    {
        _bufferLen = 0;
        _pos = 0;
        _line = 1;
    }

    public void SetSource(byte[] buffer, uint size)
    {
        if(buffer.Length <= 0)
            return;

        _buffer = buffer;
        _bufferUnicode = Encoding.Unicode.GetChars(_buffer);

        //int size2 = _bufferUnicode.Length;

        _bufferLen = (uint)_bufferUnicode.Length/*size*/;
        _pos = 0;
        _line = 1;
    }

    public int GetNextToken(ref Token token)
    {
        int type = GetNextString(ref token);

        if(type == (int)eTOKEN.eTOKEN_VAR)
        {
            type = GetTokenType(token._str);
            token._type = type;
        }

        return type;
    }

    public int GetNextString(ref Token token)
    {
        char[] lexeme = new char[512];
        for (int l = 0; l < 512; ++l)
            lexeme[l] = '\0';

        int state = (int)eLEXER.eIN_START;
        int type = (int)eTOKEN.eTOKEN_NULL;
        int i = 0;
        char c;
        bool save = false;

        while(state != (int)eLEXER.eIN_DONE)
        {
            c = GetNextChar();
            save = false;

            if(c == 0 || c == 65279)
            {
                lexeme[i] = '\0';
                token._str = new string(lexeme);
                token._type = type;
                token._line = _line;

                return type;
            }

            switch(state)
            {
                case (int)eLEXER.eIN_START:
                    {
                        if(c >= GetLexTableSize())
                        {
                            save = true;
                            state = (int)eLEXER.eIN_DONE;
                            type = (int)eTOKEN.eTOKEN_ERROR;
                            break;
                        }

                        if(gLexTable[c].mType != (int)eTOKEN.eTOKEN_NULL)
                        {
                            save = true;
                            state = (int)eLEXER.eIN_DONE;
                            type = gLexTable[c].mType;
                        }
                        else if(gLexTable[c].mState != (int)eLEXER.eIN_NULL)
                        {
                            state = gLexTable[c].mState;
                            if (state != (int)eLEXER.eIN_STRING)
                                save = true;
                        }

                        i = 0;
                    }
                    break;
                case (int)eLEXER.eIN_NUM:
                    {
                        if (char.IsNumber(c))
                        {
                            save = true;
                            if (type == (int)eTOKEN.eTOKEN_NULL)
                                type = (int)eTOKEN.eTOKEN_INT;
                        }
                        else if (c == '.')
                        {
                            save = true;
                            state = (int)eLEXER.eIN_FLOAT;
                            type = (int)eTOKEN.eTOKEN_FLOAT;
                        }
                        else
                        {
                            UngetNextChar();
                            state = (int)eLEXER.eIN_DONE;
                            type = (int)eTOKEN.eTOKEN_INT;
                        }
                    }
                    break;
                case (int)eLEXER.eIN_FLOAT:
                    {
                        if (char.IsNumber(c))
                        {
                            save = true;
                        }
                        else
                        {
                            UngetNextChar();
                            state = (int)eLEXER.eIN_DONE;
                            type = (int)eTOKEN.eTOKEN_FLOAT;
                        }
                    }
                    break;
                case (int)eLEXER.eIN_VAR:
                    {
                        if (char.IsLetter(c) || char.IsNumber(c) || c == '_' || c == '#')
                        {
                            save = true;
                        }
                        else
                        {
                            UngetNextChar();
                            state = (int)eLEXER.eIN_DONE;
                            type = (int)eTOKEN.eTOKEN_VAR;
                        }
                    }
                    break;
                case (int)eLEXER.eIN_STRING:
                    {
                        if (c != '"')
                        {
                            save = true;
                        }
                        else
                        {
                            state = (int)eLEXER.eIN_DONE;
                            type = (int)eTOKEN.eTOKEN_STR;
                        }
                    }
                    break;
                case (int)eLEXER.eIN_MINUS:
                    {
                        if (char.IsNumber(c))
                        {
                            save = true;
                            state = (int)eLEXER.eIN_NUM;
                        }
                        else if (c == '-')
                        {
                            state = (int)eLEXER.eIN_COMMENT;
                        }
                    }
                    break;
                case (int)eLEXER.eIN_DIV:
                    {
                        if (c == '/')
                        {
                            state = (int)eLEXER.eIN_COMMENT;
                        }
                        else
                        {
                            save = true;
                            state = (int)eLEXER.eIN_DONE;
                        }
                    }
                    break;
                case (int)eLEXER.eIN_COMMENT:
                    {
                        if (c == '\n')
                        {
                            state = (int)eLEXER.eIN_START;
                        }
                    }
                    break;
                default:
                    {
                        save = true;
                        state = (int)eLEXER.eIN_DONE;
                        type = (int)eTOKEN.eTOKEN_ERROR;
                    }
                    break;
            }

            if (save && i < 511)
            {
                lexeme[i] = c;
                ++i;
            }
        }

        lexeme[i] = '\0';
        token._str = new string(lexeme);
        token._type = type;
        token._line = _line;

        return type;
    }

    public int GetTokenType(string keyword)
    {
        foreach (KeyValuePair<string, int> pair in _keywordMap)
        {
            if(string.Compare(keyword, pair.Key) == 0)
            {
                return pair.Value;
            }
        }

        return (int)eTOKEN.eTOKEN_VAR;
    }

    public bool IsEnd()
    {
        while (_pos < _bufferLen)
        {
            //char c = Convert.ToChar(_buffer[_pos]);
            //char[] buffer = Encoding.Unicode.GetChars(_buffer);
            char c = _bufferUnicode[_pos];
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
            else if (c == '\xFEFF')
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

    public char GetNextChar()
    {
        if (_pos < _bufferLen)
        {
            //char[] buffer = Encoding.Unicode.GetChars(_buffer);
            char c = _bufferUnicode[_pos];
            if (c == '\n')
            {
                ++_line;
            }
            ++_pos;
            return c;
        }
        return '\0';
    }

    public void UngetNextChar()
    {
        --_pos;
        //char[] buffer = Encoding.Unicode.GetChars(_buffer);
        char c = _bufferUnicode[_pos];
        if (c == '\n')
        {
            --_line;
        }
    }

    public void BindKeyword(string keyword, int tokenType)
    {
        _keywordMap.Add(keyword, tokenType);
    }
}