using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRISLib
{
    public class Tokenizer
    {
        readonly static int DELIMITERS_SIZE = 256;

        protected string _fileName;
        protected byte[] _buffer;
        protected uint _bufferLen;
        protected bool[] _delimiters;
        protected uint _pos;
        protected int _line;

        public Tokenizer()
        {
            _fileName = string.Empty;

            Array.Clear(_buffer, 0, _buffer.Length);

            _bufferLen = 0;

            _pos = 0;
            _line = 1;

            _delimiters = new bool[DELIMITERS_SIZE];
            for (int i = 0; i < DELIMITERS_SIZE; i++)
            {
                _delimiters[i] = false;
            }
        }

        public void SetSource(byte[] buffer, uint size, bool[] delimiters, string fileName)
        {
            if (buffer.Length <= 0)
                return;

            if (size <= 0)
                return;

            _fileName = fileName;
            _buffer = buffer;
            _bufferLen = size;

            SetDelimiters(delimiters);
        }

        protected void SetDelimiters(bool[] delimiters)
        {
            _delimiters = new bool[DELIMITERS_SIZE];
            for (int i = 0; i < DELIMITERS_SIZE; i++)
            {
                _delimiters[i] = delimiters[i];
            }
        }

        protected bool IsComment(byte b)
        {
            byte b1 = _buffer[_pos + 1];
            return ((b == '/' && b1 == '/') || (b == '-' && b1 == '-'));
        }

        protected bool IsDelimiter(byte b)
        {
            return _delimiters[b + 128];
        }

        public bool GetNext(ref string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            if (_pos >= _bufferLen)
                return false;

            byte[] lexeme = new byte[256];
            for (int l = 0; l < 256; l++)
                lexeme[l] = 0;

            int i = 0;
            byte b = 0;

            for(; _pos < _bufferLen; ++_pos)
            {
                b = _buffer[_pos];
                if(b == '\n')
                {
                    ++_line;
                }
                else if(IsComment(b))
                {
                    for(_pos += 2; _pos < _bufferLen; ++_pos)
                    {
                        b = _buffer[_pos];
                        if(b == '\n')
                        {
                            ++_line;

                            break;
                        }
                    }
                    if(i == 0)
                    {
                        continue;
                    }
                    else
                    {
                        ++_pos;
                        break;
                    }
                }
                if(IsDelimiter(b))
                {
                    if (i == 0)
                    {
                        continue;
                    }
                    else
                    {
                        ++_pos;
                        break;
                    }
                }
                if( i < 255)
                {
                    lexeme[i] = b;
                    ++i;
                }
            }

            lexeme[i] = 0;
            str = System.Text.Encoding.UTF8.GetString(lexeme, 0, lexeme.Length);
            return i != 0;
        }

        public void GotoNextLine()
        {
            for (; _pos < _bufferLen; ++_pos)
            {
                byte b = _buffer[_pos];
                if (b == '\n')
                {
                    ++_line;
                    ++_pos;

                    break;
                }
            }
        }

        public byte GetTail()
        {
            if (_pos >= _bufferLen)
                return 0;
            else
                return _buffer[_pos];
        }

        public bool IsEnd()
        {
            uint pos = _pos;
            byte b = 0;

            for (; _pos < _bufferLen; ++_pos)
            {
                b = _buffer[_pos];

                if (b == '\n' || b == ' ')
                {
                    continue;
                }

                if (IsComment(b))
                {
                    for (_pos += 2; _pos < _bufferLen; ++_pos)
                    {
                        b = _buffer[_pos];

                        if (b == '\n')
                            break;
                    }

                    continue;
                }

                if (IsDelimiter(b))
                {
                    continue;
                }

                _pos = pos;
                return false;
            }

            _pos = pos;
            return true;
        }
    }
}
