using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 public class Tokenizer
 {
     private static readonly int DELIMITERS_SIZE = 256;

     protected string _fileName;
     protected byte[] _buffer;
     protected uint _bufferLen;
     protected bool[] _delimiters;
     protected uint _pos;
     protected int _line;

     public Tokenizer()
     {
         _fileName = string.Empty;

         //Array.Clear(_buffer, 0, _buffer.Length);

         _bufferLen = 0;

         _pos = 0;
         _line = 1;

         _delimiters = new bool[DELIMITERS_SIZE];
         for (int i = 0; i < DELIMITERS_SIZE; i++)
         {
             _delimiters[i] = false;
         }
     }

     public void SetSource(byte[] buffer, uint size, char[] delimiters, string fileName)
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

     protected void SetDelimiters(char[] delimiters)
     {
         _delimiters = new bool[DELIMITERS_SIZE];
         for (int i = 0; i < DELIMITERS_SIZE; i++)
         {
             _delimiters[i] = false;
         }

         for (int i = 0; i < delimiters.Length; i++)
         {
             char c = delimiters[i];
             _delimiters[c + 128] = true;
         }
     }

     protected bool IsComment(char c)
     {
         char c1 = Convert.ToChar(_buffer[_pos + 1]);
         return ((c == '/' && c1 == '/') || (c == '-' && c1 == '-'));
     }

     protected bool IsDelimiter(char c)
     {
         return _delimiters[c + 128];
     }

     public bool GetNext(ref string str)
     {
         if (_pos >= _bufferLen)
             return false;

         char[] lexeme = new char[256];
         for (int l = 0; l < 256; l++)
             lexeme[l] = '\0';

         int i = 0;
         char c = '\0';

         for (; _pos < _bufferLen; ++_pos)
         {
             c = Convert.ToChar(_buffer[_pos]);
             if (c == '\n')
             {
                 ++_line;
             }
             else if (IsComment(c))
             {
                 for (_pos += 2; _pos < _bufferLen; ++_pos)
                 {
                     c = Convert.ToChar(_buffer[_pos]);
                     if (c == '\n')
                     {
                         ++_line;

                         break;
                     }
                 }

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

             if (IsDelimiter(c))
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

             if (i < 255)
             {
                 lexeme[i] = c;
                 ++i;
             }
         }

         lexeme[i] = '\0';
         str = new string(lexeme);

         return i != 0;
     }

     public void GotoNextLine()
     {
         for (; _pos < _bufferLen; ++_pos)
         {
             char c = Convert.ToChar(_buffer[_pos]);
             if (c == '\n')
             {
                 ++_line;
                 ++_pos;

                 break;
             }
         }
     }

     public char GetTail()
     {
         return (_pos >= _bufferLen) ? '\0' : Convert.ToChar(_buffer[_pos]);
     }

     public bool IsEnd()
     {
         uint pos = _pos;
         char c = '\0';

         for (; _pos < _bufferLen; ++_pos)
         {
             c = Convert.ToChar(_buffer[_pos]);

             if (c == '\n' || c == ' ')
             {
                 continue;
             }

             if (IsComment(c))
             {
                 for (_pos += 2; _pos < _bufferLen; ++_pos)
                 {
                     c = Convert.ToChar(_buffer[_pos]);

                     if (c == '\n')
                         break;
                 }

                 continue;
             }

             if (IsDelimiter(c))
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
