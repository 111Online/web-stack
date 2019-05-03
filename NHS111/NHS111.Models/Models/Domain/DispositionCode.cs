namespace NHS111.Models.Models.Domain {
    using System;

    public class DispositionCode {
        public const string Prefix = "Dx";

        public string Value { get; private set; }

        public DispositionCode(int dxCode)
            : this("Dx" + dxCode) { }

        public DispositionCode(string dxCode) {
            var lower = dxCode.ToLower();

            if (!IsParsable(dxCode))
                throw new ArgumentException(string.Format(
                    "The provided Dx code ({0}) doesn't appear to match the expected pattern of Dx### where ### is an integer.",
                    dxCode));

            Value = lower.Replace("dx", "").Insert(0, Prefix);
        }

        public static bool IsParsable(string dxCode) {
            var lower = dxCode.ToLower();

            var number = lower.Replace("dx", "");
            int result;
            return int.TryParse(number, out result);
        }

        public int DosCode {
            get {
                var code = Value.Replace("Dx", "");
                if (code.Length == 3) {
                    if (code.StartsWith("1"))
                        return Convert.ToInt32("1" + code);
                    else
                        return Convert.ToInt32("11" + code);
                }

                return Convert.ToInt32("10" + code);
            }


        }

        public static DispositionCode Dx02 { get { return new DispositionCode("Dx02"); } }
        public static DispositionCode Dx94 { get { return new DispositionCode("Dx94"); } }
        public static DispositionCode Dx012 { get { return new DispositionCode("Dx012"); } }

        public static DispositionCode Dx89 { get { return new DispositionCode("Dx89"); } }

        public static DispositionCode Dx334 { get { return new DispositionCode("Dx334"); } }
        public static DispositionCode Dx333 { get { return new DispositionCode("Dx333"); } }

    }
}