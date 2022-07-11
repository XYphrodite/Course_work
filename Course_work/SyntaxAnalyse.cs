using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_work
{
    internal class SyntaxAnalyse
    {
        List<int> codeIn = new List<int>();
        List<int> codeOut = new List<int>();
        List<string> strings = new List<string>();
        private bool hasErr = false;
        string toOut = "";
        int newI = 0;
        int strNum = 0;
        public SyntaxAnalyse(List<int> codeIn, List<string> strings)
        {
            this.codeIn = codeIn;
            this.strings = strings;
        }
        public void Analyse()
        {
            FindMainFun();
            if (hasErr)
                return;
            FindBodyFun();
            if (hasErr)
                return;
            FindVarInitilization();
            codeOut.Add(105);
            for (int j = newI; j < codeIn.Count; j++)
            {
                if (codeIn[j] == 104)
                {
                    FindScan();
                    j = newI;
                    if (hasErr)
                        return;
                    continue;
                }
                else if ((codeIn[j] == 107) && (codeIn[j - 1] == 5))
                {
                    newI++;
                    FindAssignment();
                }
                else if (codeIn[j] == 105)
                {
                    newI = j;
                    FindLoop();
                    j = newI;
                }
            }
            codeOut.Add(112);
            codeOut.Add(11);
            toOut += ">Синтаксический анализ завершён\n";
        }
        private void FindMainFun()
        {
            if ((codeIn[0] == 101) || (codeIn[1] == 1) || (codeIn[2] == 2))
            {
                toOut += ">Найден вход в главную функцию\n";
                codeOut.Add(101);
                codeOut.Add(1);
            }
            else
            {
                toOut += "!Не найден вход в главную функцию!\n";
                hasErr = true;
            }
        }
        private void FindBodyFun()
        {
            int leftFB = 0, rightFB = 0;
            if (codeIn[3] == 3)
            {
                newI = 3;
            }
            else
            {
                toOut += "!Не найдено тело функции\n";
            }
            for (int i = newI; i < codeIn.Count; i++)
            {
                if (codeIn[i] == 3)
                {
                    leftFB++;

                }
                if (codeIn[i] == 4)
                    rightFB++;
            }
            if (leftFB != rightFB)
            {
                hasErr = true;
                toOut += "!Не корректное тело функции!\n";
                return;
            }
        }
        private void FindVarInitilization()
        {
            int integer = 0;
            int d = 0;
            codeOut.Add(102);
            for (int i = newI; i < codeIn.Count; i++)
            {
                if ((codeIn[i] == 102))
                {
                    i++;
                    while (codeIn[i] != 5)
                    {
                        if (codeIn[i] == 107)
                        {
                            integer++;
                            codeOut.Add(113);
                        }
                        if (codeIn[i] == 6)
                        {
                            codeOut.Add(3);
                        }
                        i++;
                    }
                    newI = i + 1;
                    codeOut.Add(2);
                    codeOut.Add(103);
                    codeOut.Add(1);
                }
                if ((codeIn[i] == 103))
                {
                    i++;
                    while (codeIn[i] != 5)
                    {
                        if (codeIn[i] == 107)
                        {
                            d++;
                            codeOut.Add(113);
                        }
                        if (codeIn[i] == 6)
                        {
                            codeOut.Add(3);
                        }
                        i++;
                    }
                    newI = i + 1;
                    codeOut.Add(2);
                    codeOut.Add(104);
                    codeOut.Add(1);
                }
            }

            toOut += ">Проработана инициализация переменных\n";
        }
        private void FindScan()
        {
            codeOut.Add(106);
            List<int> scanFun = new List<int>();
            codeOut.Add(9);
            newI++;
            if (codeIn[newI] == 1)
            {
                newI++;
                while (codeIn[newI] != 2)
                {
                    scanFun.Add(codeIn[newI]);
                    newI++;
                }
            }
            bool hasStr = false;
            int amountVar = 0;
            int amountPar = 0;
            for (int i = 0; i < scanFun.Count; i++)
            {
                if (scanFun[i] == 109)
                {
                    hasStr = true;
                }
                else if (scanFun[i] == 107)
                {
                    amountVar++;
                    if (scanFun[i - 1] == 7)
                    {
                        codeOut.Add(113);
                    }
                    else
                    {
                        toOut += "!Ошибка в передаче параметра функции чтения с клавиатуры!\n";
                        hasErr = true;
                    }
                }
                else if (scanFun[i] == 7)
                {
                    continue;
                }
                else if (scanFun[i] == 6)
                {
                    if (amountVar!=0)
                        codeOut.Add(3);
                }
                else
                {
                    toOut += "!Ошибка в передаче параметра функции чтения с клавиатуры!\n";
                    hasErr = true;
                }
            }
            if (hasStr)
            {
                while (strings[strNum].Contains("%le"))
                {
                    strings[strNum] = strings[strNum].Remove(strings[strNum].IndexOf('%'), 3);
                    amountPar++;
                }
                if ((strings[strNum] != "\"\"") || (amountPar != amountVar))
                {
                    hasErr = true;
                    toOut += "!Ошибка в передаче параметра функции чтения с клавиатуры!\n";
                }
            }
            codeOut.Add(10);
            newI++;
            if (codeIn[newI] == 5)
                codeOut.Add(1);
            else
            {
                hasErr = true;
                toOut += "!Не найдена точка с запятой после функции чтения с клавиатуры!\n";
            }
            toOut += ">Завершён анализ функции ввода\n";
            strNum++;
        }
        private void FindAssignment()
        {
            List<int> expression = new List<int>();
            while (codeIn[newI] != 5)
            {
                expression.Add(codeIn[newI]);
                newI++;
            }
            solveExpressionIssue(expression);
            codeOut.Add(1);
            newI++;
            toOut += ">Присваивание завершено\n";
        }
        private void FindAssignment(int i, List<int> loop)
        {
            List<int> expression = new List<int>();
            int newj = i;
            while (loop[newj] != 5)
            {
                expression.Add(loop[newj]);
                newj++;
            }
            solveExpressionIssue(expression);
            codeOut.Add(1);
            newj++;
            toOut += ">Присваивание завершено\n";
        }
        private void FindLoop()
        {
            codeOut.Add(107);
            newI++;
            List<int> loop = new List<int>();
            if (codeIn[newI] == 1)
            {
                newI++;
                while (codeIn[newI] != 2)
                {
                    loop.Add(codeIn[newI]);
                    newI++;
                }
                newI++;
            }
            else
            {
                hasErr = true;
                toOut += "!Ошибка в цикле(условие)!\n";
            }
            solveConditionIssue(loop);
            loop.Clear();
            codeOut.Add(109);
            if (codeIn[newI] == 3)
            {
                codeOut.Add(105);
                while (codeIn[newI] != 4)
                {
                    loop.Add(codeIn[newI]);
                    newI++;
                }
                newI++;
                for (int j = 0; j < loop.Count; j++)
                {
                    if (loop[j] == 106)
                    {
                        FindWrite(j, loop);
                        j = newI;
                        if (hasErr)
                            continue;
                    }
                    else if ((loop[j] == 107) && ((loop[j - 1] == 5) || (loop[j - 1] == 3)))
                    {
                        FindAssignment(j, loop);
                    }
                }
                codeOut.Add(112);
            }
            else
            {
                hasErr = true;
                toOut += "!Ошибка в цикле(тело)!\n";
            }
            toOut += ">Завершён анализ цикла\n";
        }
        private void solveExpressionIssue(List<int> expression)
        {
            int[] pmmd = { 5, 6, 7, 8, 1, 2 };
            try
            {
                if (expression[1] != 8)
                {
                    hasErr = true;
                    toOut += "!Ошибка при присваивании!\n";
                }
            }
            catch
            {
                hasErr = true;
                toOut += "!Ошибка при присваивании!\n";
            }
            if (expression[0] != 107)
            {
                hasErr = true;
                toOut += "!Ошибка при присваивании!\n";
            }
            for (int i = 0; i < expression.Count; i++)
            {
                switch (expression[i])
                {
                    case 107:
                        codeOut.Add(113);
                        break;
                    case 108:
                        codeOut.Add(114);
                        break;
                    case 8:
                        codeOut.Add(4);
                        break;
                    case 9:
                        codeOut.Add(5);
                        break;
                    case 10:
                        codeOut.Add(6);
                        break;
                    case 12:
                        codeOut.Add(7);
                        break;
                    case 14:
                        codeOut.Add(8);
                        break;
                    case 1:
                        codeOut.Add(9);
                        break;
                    case 2:
                        codeOut.Add(10);
                        break;
                    default:
                        hasErr = true;
                        toOut += "!Ошибка при присваивании!\n";
                        break;
                }
            }
        }
        private void solveConditionIssue(List<int> loop)
        {
            List<int> help = new List<int>();
            int j = 0;
            while (loop[j] != 5)
            {
                help.Add(loop[j]);
                j++;
            }
            solveExpressionIssue(help);
            help.Clear();
            j++;
            while (loop[j] != 5)
            {
                if (loop[j] == 107)
                {
                    codeOut.Add(1150);
                }
                else if ((loop[j] == 11) && (loop[j + 1] == 8))
                {
                    codeOut.Add(108);

                    j++;
                }
                else if (loop[j] == 108)
                    codeOut.Add(114);
                else
                {
                    hasErr = true;
                    toOut += "!Ошибка в цикле(условие)!\n";
                }
                j++;
            }
            j++;
            help.Clear();
            while (j < loop.Count)
            {
                try
                {
                    if ((loop[j] == 107) && (loop[j + 1] == 12) && (loop[j + 2] == 12))
                    {
                        j += 3;
                        codeOut.Add(1150);
                    }
                }
                catch
                {
                    hasErr = true;
                    toOut += "!Ошибка в цикле(условие)!\n";
                    j++;
                }
            }
            toOut += ">Завершён анализ условия цикла\n";
        }
        private void FindWrite(int j, List<int> loop)
        {
            if (strings[strNum].Contains("\\n"))
                codeOut.Add(111);
            else
                codeOut.Add(110);
            List<int> scanFun = new List<int>();
            j++;
            if (loop[j] == 1)
            {
                codeOut.Add(9);
                j++;
                while (loop[j] != 2)
                {
                    scanFun.Add(loop[j]);
                    j++;
                }
            }
            j++;
            bool hasStr = false;
            for (int i = 0; i < scanFun.Count; i++)
            {
                if (scanFun[i] == 109)
                {
                    hasStr = true;
                }
                else if (scanFun[i] == 107)
                {

                }
                else if (scanFun[i] == 6)
                {

                }
                else
                {
                    toOut += "!Ошибка в передаче параметра функции вывода!\n";
                    hasErr = true;
                }
            }
            if (hasStr)
            {
                string newstr="";
                int varAmount = 0, parAmount = 0;
                foreach(var l in scanFun)
                {
                    if (l==113)
                        varAmount++;
                }
                for (int i = 0; i < strings[strNum].Length; i++)
                {
                    if ((strings[strNum][i]=='%')&& (strings[strNum][i+1] == 'l')&& (strings[strNum][i+2] == 'e'))
                    {
                        if (parAmount != 0)
                            codeOut.Add(3);
                        i += 2;
                        codeOut.Add(113);
                        
                        parAmount++;
                    }
                    else if (strings[strNum][i] == ',')
                    {
                        if (parAmount != 0)
                            codeOut.Add(3);
                        codeOut.Add(115);
                        newstr += "','";
                    }
                    
                }
                strings[strNum] = newstr;
            }
            else
            {
                hasErr = true;
                toOut += "!Ошибка в передаче параметра функции вывода!\n";
            }
            codeOut.Add(10);
            try
            {
                if (loop[j] == 5)
                    codeOut.Add(1);
            }
            catch
            {
                hasErr = true;
                toOut += "!Не найдена точка с запятой после функции вывода!\n";
            }
            toOut += ">Завершён анализ функции вывода\n";
        }
        public string GetToOut()
        {
            return toOut;
        }
        public List<int> GetCode()
        {
            
            if (hasErr)
            {
                return null;
            }
            return codeOut;
        }
        public List<string> GetStrings()
        {
            return strings;
        }
    }
}