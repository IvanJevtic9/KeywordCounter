using System;
using System.Collections.Generic;
using System.Text;

namespace KeyWordCounter
{
    public static class Utils
    {
        public static Dictionary<string, int> Initialize(this Dictionary<string,int> dir)
        {
            foreach (var word in ApplicationSettings.Instance.KeyWords)
            {
                dir.Add(word, 0);
            }
            return dir;
        }
    }
}