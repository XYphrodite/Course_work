using System.Collections.Generic;
using System.IO;

namespace Course_work
{
    internal class LexicalAnalyse
    {
        public LexicalAnalyse(string codeIn)
        {
            this.codeIn = codeIn;
        }
        Dictionary<string, int> tokenCode = new Dictionary<string, int>()
        {
            ["("] = 1,
            [")"] = 2,
            ["{"] = 3,
            ["}"] = 4,
            [";"] = 5,
            [","] = 6,
            ["&"] = 7,
            ["="] = 8,
            ["-"] = 9,
            ["/"] = 10,
            ["<"] = 11,
            ["+"] = 12,
            ["++"] = 13,
            ["*"] = 14,
            ["main"] = 101,
            ["int"] = 102,
            ["double"] = 103,
            ["scanf"] = 104,
            ["for"] = 105,
            ["printf"] = 106,
            ["id"] = 107,
            ["const"] = 108,
            ["string"] = 109
        };
        List<char> uselessSymbols = new List<char>() { '\t', '\n', ' ' };
        List<char> specialSymbols = new List<char>() { ';', '*', '-', ',', '/', '+', ' ', '=', ')', '<' };
        string codeIn;
        List<int> codeOut = new List<int>();
        List<int> consts = new List<int>();
        List<string> ids = new List<string>();
        List<string> strings = new List<string>();
        
        public void Analyse()
        {
            Split();
        }

        private void Split()
        {
            string token = "";
            int t, ht;
            for (int i = 0; i < codeIn.Length; i++)
            {
                if (uselessSymbols.Contains(codeIn[i]))
                    continue;
                if (tokenCode.TryGetValue(codeIn[i].ToString(), out t))
                {
                    token = "";
                    codeOut.Add(t);
                    continue;
                }
                if (codeIn[i] == '\"')
                {
                    token = "";
                    while (codeIn[i + 1] != '\"')
                    {
                        token += codeIn[i].ToString();
                        i++;
                    }
                    token += codeIn[i].ToString();
                    token += codeIn[i + 1].ToString();
                    strings.Add(token);
                    token = "";
                    i++;
                    codeOut.Add(109);
                    continue;
                }
                token += codeIn[i];
                if (tokenCode.TryGetValue(token, out t))
                {
                    token = "";
                    codeOut.Add(t);
                }
                else if (int.TryParse(token, out t))
                {
                    codeOut.Add(108);
                    while (int.TryParse(codeIn[i + 1].ToString(), out ht))
                    {
                        t = t*10 + ht;
                        i++;
                    }
                    consts.Add(t);
                    token = "";
                }
                else if (i + 1 < codeIn.Length)
                {
                    if (specialSymbols.Contains(codeIn[i + 1]))
                    {
                        codeOut.Add(107);
                        ids.Add(token);
                        token = "";
                    }
                }

            }
        }

        public List<int> GetCodeOut()
        {
            return codeOut;
        }
        public List<string> GetIds()
        {
            return ids;
        }
        public List<int> GetConsts()
        {
            return consts;
        }
        public List<string> GetStrings()
        {
            return strings;
        }
    }
}
