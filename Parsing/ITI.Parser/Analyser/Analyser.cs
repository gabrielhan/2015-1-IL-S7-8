﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class Analyser
    {
        ITokenizer _tokenizer;

        public Node Analyse( ITokenizer tokenizer )
        {
            _tokenizer = tokenizer;
            if( _tokenizer.CurrentToken == TokenType.None ) _tokenizer.GetNextToken();
            return HandleExpression();
        }

        private Node HandleExpression()
        {
            var left = HandleTerm();
            while( _tokenizer.CurrentToken == TokenType.Plus || _tokenizer.CurrentToken == TokenType.Minus )
            {
                var type = _tokenizer.CurrentToken;
                _tokenizer.GetNextToken();
                left = new BinaryNode( type, left, HandleTerm() );
            }
            return left;
        }

        private Node HandleTerm()
        {
            var left = HandleFactor();
            while( _tokenizer.CurrentToken == TokenType.Mult || _tokenizer.CurrentToken == TokenType.Div )
            {
                var type = _tokenizer.CurrentToken;
                _tokenizer.GetNextToken();
                left = new BinaryNode( type, left, HandleFactor() );
            }
            return left;
        }

        private Node HandleFactor()
        {
            bool isNeg = _tokenizer.Match( TokenType.Minus );
            var e = HandlePositiveFactor();
            return isNeg ? new UnaryNode( TokenType.Minus, e ) : e;
        }
        
        private Node HandlePositiveFactor()
        {
            double numberValue;
            if( _tokenizer.MatchDouble( out numberValue ) ) return new ConstantNode( numberValue );
            if( _tokenizer.Match( TokenType.OpenPar ) )
            {
                var e = HandleExpression();
                if( !_tokenizer.Match( TokenType.ClosePar ) ) return new ErrorNode( "Expected )." );
                return e;
            }
            return new ErrorNode( "Expected number or (expression)." );
        }
    }
}



namespace gaby.Parser
{
    public class Analyser
    {
        ITokenizer _tokenizer;

        public Node Analyse(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            if (_tokenizer.CurrentToken == TokenType.None) _tokenizer.GetNextToken();
            return HandleExpression();
        }

        private Node HandleExpression()
        {
            var left = HandleTerm();
            while (_tokenizer.CurrentToken == TokenType.Plus || _tokenizer.CurrentToken == TokenType.Minus)
            {
                var type = _tokenizer.CurrentToken;
                _tokenizer.GetNextToken();
                left = new BinaryNode(type, left, HandleTerm());
            }
            return left;
        }

        private Node HandleTerm()
        {
            var left = HandleFactor();
            while (_tokenizer.CurrentToken == TokenType.Mult || _tokenizer.CurrentToken == TokenType.Div)
            {
                var type = _tokenizer.CurrentToken;
                _tokenizer.GetNextToken();
                left = new BinaryNode(type, left, HandleFactor());
            }
            return left;
        }

        private Node HandleFactor()
        {
            bool isNeg = _tokenizer.Match(TokenType.Minus);
            var e = HandlePositiveFactor();
            return isNeg ? new UnaryNode(TokenType.Minus, e) : e;
        }

        private Node HandlePositiveFactor()
        {
            double numberValue;
            if (_tokenizer.MatchDouble(out numberValue)) return new ConstantNode(numberValue);
            if (_tokenizer.Match(TokenType.OpenPar))
            {
                var e = HandleExpression();
                if (!_tokenizer.Match(TokenType.ClosePar)) return new ErrorNode("Expected ).");
                return e;
            }
            return new ErrorNode("Expected number or (expression).");
        }
    }
}