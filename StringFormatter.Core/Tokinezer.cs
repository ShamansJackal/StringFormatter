using StringFormatter.Core.Tokens;

namespace StringFormatter.Core
{
    internal class Tokinezer
    {
        public static IEnumerable<BaseToken> GetTokens(string input)
        {
            int pos = 0;
            sbyte currentState = 1;
            List<BaseToken> tokens = new();

            for (int i = 0; i < input.Length; i++)
            {
                sbyte nextState = _transitionTable[currentState, GetSymbleCode(input[i])];

                if (nextState < 0)
                {
                    nextState = Math.Abs(nextState);


                    tokens.Add(
                        _tokenTypeByTransition[currentState, GetSymbleCode(input[i])] switch
                        {
                            0 => new TextToken() { TokenText = input[pos..i] },
                            1 => new EscapeToken() { TokenText = input[pos..++i] },
                            2 => new ReplaceToken() { TokenText = input[pos..++i] },
                        }
                    );

                    pos = i;
                }else if(currentState == 0)
                {
                    throw new Exception("Syntax error");
                }

                currentState = nextState;
            }

            if (pos != input.Length)
                tokens.Add(new TextToken() { TokenText = input[pos..] });

            return tokens;
        }


        private readonly static sbyte[,] _transitionTable = new sbyte[5, 3]
        {
            {0,0,0}, //Err state
            {-2,-4,1  }, //A
            {-1,0,3},   //B
            {0,-1,3},    //C
            {0,-1,0 }    //D
        };

        private readonly static sbyte[,] _tokenTypeByTransition = new sbyte[5, 3]
        {
            {-1,-1,-1 },
            {0, 0,-1 },
            {1, -1,-1 },
            {-1,2,-1 },
            {-1,1,-1 }
        };

        private static sbyte GetSymbleCode(char symb) =>symb 
            switch
            {
                '{' => 0,
                '}' => 1,
                _ => 2,
            };
    }
}