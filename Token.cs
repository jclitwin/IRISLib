using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRISLib
{
    public class Token
    {
        /// Token type
        public enum eTOKEN
        {
            eTOKEN_ERROR = -1, /// Error
            eTOKEN_NULL = 0,   /// Null
            eTOKEN_INT,        /// Int
            eTOKEN_FLOAT,      /// Float
            eTOKEN_VAR,        /// Var
            eTOKEN_STR,        /// String
            eTOKEN_NOT,        /// !
            eTOKEN_SHARP,      /// #
            eTOKEN_DOLLAR,     /// $
            eTOKEN_PERCENT,    /// %
            eTOKEN_AND,        /// &
            eTOKEN_LPAREN,     /// (
            eTOKEN_RPAREN,     /// )
            eTOKEN_MUL,        /// *
            eTOKEN_PLUS,       /// +
            eTOKEN_COMMA,      /// ,
            eTOKEN_DOT,        /// .
            eTOKEN_DIV,        /// /
            eTOKEN_COLON,      /// :
            eTOKEN_SEMICOLON,  /// ;
            eTOKEN_LANGLE,     /// <
            eTOKEN_ASSIGN,     /// =
            eTOKEN_RANGLE,     /// >
            eTOKEN_QUESTION,   /// ?
            eTOKEN_AT,         /// @
            eTOKEN_LSQUARE,    /// [
            eTOKEN_RSQUARE,    /// ]
            eTOKEN_XOR,        /// ^
            eTOKEN_LCURLY,     /// {
            eTOKEN_OR,         /// |
            eTOKEN_RCURLY,     /// }
            eTOKEN_TILD,       /// ~
        };
    }
}
