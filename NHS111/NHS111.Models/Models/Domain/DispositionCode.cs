namespace NHS111.Models.Models.Domain {
    using System;

    public class DispositionCode {
        public const string Prefix = "Dx";

        public string Value { get; private set; }

        public DispositionCode(string dxCode) {
            var lower = dxCode.ToLower();

            if(!IsParsable(dxCode))
                throw new ArgumentException(string.Format("The provided Dx code ({0}) doesn't appear to match the expected pattern of Dx### where ### is an integer.", dxCode));

            Value = lower.Replace("dx", "").Insert(0, Prefix);
        }

        public static bool IsParsable(string dxCode) {
            var lower = dxCode.ToLower();

            var number = lower.Replace("dx", "");
            int result;
            return int.TryParse(number, out result);
        }
    }
}