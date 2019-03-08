using UnityEngine;
using UnityEngine.UI;

public class SquasherCtlr : MonoBehaviour {

  #pragma warning disable 0649
  [SerializeField] private Slider    _sldW;
  [SerializeField] private Slider    _sldL;
  [SerializeField] private Slider    _sldH;
  [SerializeField] private Transform _trnObject;
  [SerializeField] private Text      _txtVolume;
  #pragma warning restore 0649
  

  void Start () {
    _sldL.onValueChanged.AddListener((unused) => OnSlider(false));
    _sldW.onValueChanged.AddListener((unused) => OnSlider(false));
    _sldH.onValueChanged.AddListener((unused) => OnSlider(true ));
    OnSlider(false);
  }
  
  void OnSlider(bool recalculateLW) {
  
    if (recalculateLW) {
      /*
      from https://answers.unity.com/questions/1609826/how-to-squash-a-rectangular-prism-without-changing.html?childToView=1610074
      Let's say your rectangular prism starts off with L, W, H,
      and you want to sqaush it to some new height h,
      and calculate the new length and width l and w.
      L and W change by the same amount, and the volume is unchanged.

      so you want to find d, the delta in L and W:

      setup:
      1. l                            = L + d
      2. w                            = W + d
      3. lwh                          = LWH

      solving:
      4. (L + d) * (W + d) * h        = LWH
      5. LW + Ld + Wd + dd            = LWH/h
      6. dd + d(L + W) +  LW          = LWH/h 
      7. dd + d(L + W) + (LW - LWH/h) = 0
      8. dd + d(L + W) + LW(1 - H/h)  = 0

      which is a quadratic equation where
      A = 1
      B = L + W
      C = L * W * (1 - H/h)
      */

      float L = _sldL.value;
      float W = _sldW.value;
      float H = _trnObject.localScale.y;
      float h = _sldH.value;
      
      float A = 1.0f;
      float B = L + W;
      float C = L * W * (1.0f - H / h);
      
      float d1, d2;
      
      int numSolutions = oxeMath.rootsQuadratic(A, B, C, out d1, out d2);
      
      float l1 = L + d1;
      float l2 = L + d2;
      float w1 = W + d1;
      float w2 = W + d2;
      
      bool valid = true;
      valid = valid && (numSolutions > 0);
      valid = valid && ((l1 > 0.0f && w1 > 0.0f) || (l1 > 0.0f && w1 > 0.0f));
      if (!valid) {
        Debug.Log("no solutions.");
      }
      else {      
        if (l1 > 0.0f && w1 > 0.0f) {
          _sldL.value = l1;
          _sldW.value = w1;
        }
        else {
          _sldL.value = l2;
          _sldW.value = w2;
        }
      }
    }
    _trnObject.localScale = new Vector3(_sldL.value, _sldH.value, _sldW.value);
    float volume = _trnObject.localScale.x * _trnObject.localScale.y * _trnObject.localScale.z;
    _txtVolume.text = volume.ToString("0.0");
  }
}
