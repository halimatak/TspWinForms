using System;

namespace TspCore
{
    public static class RandomUtil
    {
        public static Random Create(int seed) => new Random(seed);
    }
}