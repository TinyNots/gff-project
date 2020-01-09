using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menuのどこに出すかは自由.
/// </summary>
[AddComponentMenu("UI/Effects/TextKerning", 0)]
public class TextKerning : BaseMeshEffect
{
    private const int CharaVertexNum = 4;
    private const float CharaVertexNumF = 4.0f;

    /// <summary>
    /// 文字間の距離をどれだけ詰めるか.
    /// 0 で何もしない状態.
    /// 1 で100%詰め(全ての文字が重なる).
    /// </summary>
    [SerializeField]
    private float kerningRate = 0.0f;

    private Vector3[] centerList = null;

    protected TextKerning()
    {
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || kerningRate == 0.0f)
        {
            return;
        }

        int count = vh.currentVertCount;

        // 1文字は4頂点のはずなので、余があるならTextではない？.
        if ((count % CharaVertexNum) != 0)
        {
            return;
        }

        int centerNum = count / CharaVertexNum;

        if (centerList == null || centerList.Length != centerNum)
        {
            centerList = new Vector3[centerNum];
        }

        // 4頂点毎の座標の重心を計算する.
        UIVertex vert = new UIVertex();
        Vector3 vec3 = Vector3.zero;

        for (int i = 0, j = 0, cnt = 0; i < count; ++i)
        {
            vh.PopulateUIVertex(ref vert, i);

            vec3 += vert.position;

            ++cnt;
            if (cnt >= CharaVertexNum)
            {
                centerList[j] = vec3 / CharaVertexNumF;
                ++j;
                cnt = 0;
                vec3 = Vector3.zero;
            }
        }

        // 文字全体を左寄せする.
        // 右寄せやら中央寄せが欲しいなら別途処理を作るしかないが、Text のアライメント指定があるので基本不要なはず？.
        float kerning = 0.0f;

        for (int i = CharaVertexNum, j = 0, cnt = 1; i < count; i += CharaVertexNum)
        {
            vec3 = centerList[cnt - 1] - centerList[cnt];
            ++cnt;

            kerning += vec3.magnitude * kerningRate;

            for (j = 0; j < CharaVertexNum; ++j)
            {
                vh.PopulateUIVertex(ref vert, i + j);

                vec3 = vert.position;
                vec3.x -= kerning;
                vert.position = vec3;

                vh.SetUIVertex(vert, i + j);
            }
        }
    }
}