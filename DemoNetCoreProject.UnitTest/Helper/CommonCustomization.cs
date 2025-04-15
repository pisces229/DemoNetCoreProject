using AutoFixture;

namespace DemoNetCoreProject.UnitTest.Helper
{
    public class CommonCustomization : ICustomization
    {
        /// <summary>
        /// 自定義 Fixture 配置
        /// </summary>
        /// <param name="fixture">要自定義的 Fixture 實例</param>
        public void Customize(IFixture fixture)
        {
            // 移除 ThrowingRecursionBehavior，避免循環引用時拋出異常
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));

            // 添加 OmitOnRecursionBehavior，處理循環引用
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1)); // 設置樹深度為 1，避免過深的對象圖

            // 設置 RepeatCount 為 1，避免創建過多的對象
            fixture.RepeatCount = 1;
        }
    }
}
