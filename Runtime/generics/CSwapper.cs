namespace Wowsome {
  namespace Generic {
    public interface ISwappable<T> {
      T SwapData { get; set; }
    }

    public class CSwapper<T> {
      public void SetSwap(ISwappable<T> swapUnit1, ISwappable<T> swapUnit2) {
        //swap if the new one is not the same as the current one
        if (swapUnit1 != swapUnit2) {
          T temp = swapUnit1.SwapData;
          swapUnit1.SwapData = swapUnit2.SwapData;
          swapUnit2.SwapData = temp;
        }
      }
    }
  }
}