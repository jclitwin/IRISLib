using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Parser
{
    protected Lexer _lexer;
    protected string _fileName;
    protected Token _token;

    public Parser()
    {
        _lexer = null;
    }

    public Parser(Lexer lexer, string fileName)
    {
        _lexer = lexer;
        _fileName = fileName;
    }

    // test
    public Parser(Lexer lexer, string fileName, ref Token token)
    {
        _lexer = lexer;
        _fileName = fileName;
        _token = token;
    }

    public bool ExpectTokenString(string str)
    {
        _lexer.GetNextString(ref _token);
        if(string.Compare(_token._str, str) != 0)
        {
            return false;
        }

        return true;
    }

    public bool ExpectTokenType(int type)
    {
        if(_lexer.GetNextToken(ref _token) != type)
        {
            return false;
        }

        return true;
    }

    public bool ParseString(ref string str)
    {
        _lexer.GetNextString(ref _token);
        str = _token._str;
        return true;
    }

    public int ParseInt()
    {
        int type = _lexer.GetNextString(ref _token);
        if(type != (int)eTOKEN.eTOKEN_INT)
        {
            return 0;
        }

        return Int32.Parse(_token._str);
    }

    public bool ParseInt(ref int typeOut)
    {
        int type = _lexer.GetNextString(ref _token);
        if (type != (int)eTOKEN.eTOKEN_INT)
        {
            return false;
        }

        typeOut = Int32.Parse(_token._str);
        return true;
    }

    public float ParseFloat()
    {
        int type = _lexer.GetNextString(ref _token);
        if (type != (int)eTOKEN.eTOKEN_INT && type != (int)eTOKEN.eTOKEN_FLOAT)
        {
            return 0.0f;
        }

        return float.Parse(_token._str);
    }

    public bool ParseFloat(ref float typeOut)
    {
        int type = _lexer.GetNextString(ref _token);
        if (type != (int)eTOKEN.eTOKEN_INT && type != (int)eTOKEN.eTOKEN_FLOAT)
        {
            return false;
        }

        typeOut = float.Parse(_token._str);
        return true;
    }

    public void SetLexer(Lexer lexer)
    {
        _lexer = lexer;
    }

    public void SetFileName(string fileName)
    {
        _fileName = fileName;
    }

    public Lexer GetLexer()
    {
        return _lexer;
    }

    public int GetLineNum()
    {
        return _token._line;
    }

    public string GetFileName()
    {
        return _fileName;
    }
}
