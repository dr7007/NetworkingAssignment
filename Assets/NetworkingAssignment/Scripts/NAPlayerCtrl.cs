using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class NAPlayerCtrl : MonoBehaviourPun
{
    [SerializeField]
    private string nickName = null;

    private RectTransform rectTr = null;
    private TextMeshProUGUI playerUI = null;

    private void Awake()
    {
        rectTr = this.GetComponentInChildren<RectTransform>();
        playerUI = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

    }

}
