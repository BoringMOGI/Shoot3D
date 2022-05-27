using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction
{
    string GetContext();        // ǥ���� ����.
    void OnInteracation();      // ��ȣ�ۿ�.
}

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Animator anim;             // �ִϸ�����.
    [SerializeField] WeaponController weapon;   // ����.
    [SerializeField] GrenadeThrow grenadeThrow; // ����ź.
    [SerializeField] Inventory inven;           // �κ��丮.

    [Header("Eye")]
    [SerializeField] Camera eye;                // ��.
    [SerializeField] Transform normalCamera;    // �Ϲ� �þ� ��ġ.
    [SerializeField] Transform aimCamera;       // ���� �þ� ��ġ.

    [Header("Search")]
    [SerializeField] LayerMask searchMask;      // Ž�� ����ũ.
    [SerializeField] float searchRadius;        // Ž�� ����.
    [SerializeField] KeyCode interactionKey;    // ��ȣ�ۿ� Ű.

    IInteraction interaction;        // ��ȣ�ۿ� ������ ���.
    bool isAim;

    private void Start()
    {
        inven.AddItem(ItemManager.Instance.GetItem(ItemData.ITEM_TYPE.Postion, 20));
        inven.AddItem(ItemManager.Instance.GetItem(ItemData.ITEM_TYPE.Ammo, 100));
        inven.AddItem(ItemManager.Instance.GetItem(ItemData.ITEM_TYPE.Armor, 1));
    }
    void Update()
    {
        if (weapon != null && !weapon.isReload && !InventoryUI.Instance.isOpen)
        {
            Fire();
            Reload();
            Grenade();
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            // �κ��丮�� �������� ���콺 Ǯ�� ������ ���콺 ��.
            bool isOpen = InventoryUI.Instance.SwitchInventory();
            if (isOpen)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            Item item = ItemManager.Instance.GetItem(ItemData.ITEM_TYPE.Postion, 20);
            inven.AddItem(item);
        }

        // ��ȣ �ۿ� Ű ����.
        if(Input.GetKeyDown(interactionKey))
        {
            if (interaction != null)
                interaction.OnInteracation();
        }
        
        ChangeFireType();
        Aim();
        SearchInteraction();
    }

    private void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            if(weapon.StartFire(isAim))
                anim.SetTrigger("onFire");
        }
        else if (Input.GetMouseButtonUp(0))
            weapon.EndFire();
    }
    private void Aim()
    {
        if(Input.GetMouseButtonDown(1) && !weapon.isReload)
        {
            anim.SetTrigger("onAim");
        }

        isAim = Input.GetMouseButton(1) && !weapon.isReload;
        anim.SetBool("isAim", isAim);
        eye.transform.position = isAim ? aimCamera.position : normalCamera.position;
        eye.fieldOfView = isAim ? 45 : 60;
        CrossHairUI.Instance.SwitchCrosshair(!isAim);
    }
    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && weapon.Reload())
            anim.SetTrigger("onReload");
    }
    private void Grenade()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            grenadeThrow.OnThrowGrenabe();
        }
    }
    private void ChangeFireType()
    {
        if (Input.GetKeyDown(KeyCode.B))
            weapon.OnChangeType();
    }


    private void SearchInteraction()
    {
        interaction = null;

        // ���ʿ� ���� ���̸� �߻��� ��ȣ�ۿ� ��ü �˻�.
        RaycastHit hit;
        if(Physics.Raycast(eye.transform.position, eye.transform.forward, out hit, searchRadius * 2f, searchMask))
        {
            interaction = hit.collider.GetComponent<IInteraction>();
            Debug.Log("��ü Ȯ�� : " + interaction);
        }

        // �����ϸ� �������� ��˻�.
        if (interaction == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius, searchMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                // ���� �ش� ������Ʈ�� �ױװ� line_Only��� �̹� ���� ����.
                if (colliders[i].tag == "line_Only")
                    continue;

                IInteraction target = colliders[i].GetComponent<IInteraction>();
                if (target != null)
                {
                    interaction = target;
                    break;
                }
            }
        }

        // ��ȣ�ۿ� ������ ��ü�� ã�Ҵٸ�.
        if (interaction != null)
            InteractionUI.Instance.Setup(interactionKey, interaction);
        else
            InteractionUI.Instance.Close();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(eye.transform.position, eye.transform.forward * searchRadius * 2f);
    }

}
