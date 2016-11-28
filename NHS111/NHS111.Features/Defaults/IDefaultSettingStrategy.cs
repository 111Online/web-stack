namespace NHS111.Features.Defaults {

    public interface IDefaultSettingStrategy<out T> {
        T GetDefaultSetting();
    }
}