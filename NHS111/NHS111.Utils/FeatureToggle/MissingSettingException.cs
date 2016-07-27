namespace NHS111.Utils.FeatureToggle {
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MissingSettingException
        : Exception
    {

        public MissingSettingException() { }

        public MissingSettingException(string message)
            : base(message)
        { }

        public MissingSettingException(string message, Exception inner)
            : base(message, inner)
        { }

        protected MissingSettingException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        { }
    }
}