#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("jkzJm0KdA5ZCJDdu11YUdxMM1iSmXQSJ8Lf7vFDvNZLwBIA1TCpti2zyBakAZ2DMkUgswlm0rXoFgOa4BhYwuAy2+fUbaqg19tIwkK38idqs6Wra1RfkRuGXJ0soMaUeYWadnvJAw+Dyz8TL6ESKRDXPw8PDx8LBQMPNwvJAw8jAQMPDwm3v794Tgs3RVIwsiP8kH19KQDh8brJO4hKi01h3qEI0kMEW0anQ1FIc6cnbTWQE8WAMAvewOeps1Kby5+4mPeTPyY/oaqhj6Reb/6hHGJQUleqbIVUgBT6VAzZsdgvv2fLAV7YUU/PR43dmdjZz4sx0tJS2skecnf22RYeTqD0tUZwkCntByWM4EqV+J9J46tikqZqXBIUsX4sz18DBw8LD");
        private static int[] order = new int[] { 10,6,13,9,5,10,6,11,8,9,11,11,13,13,14 };
        private static int key = 194;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
