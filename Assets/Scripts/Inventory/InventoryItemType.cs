public enum InventoryItemType : System.UInt16
{
    None,

    Facility        = 0x100, //0000_0001_0000_0000,
    SimpleTurret,
    Cannon,
    EndFacility,

    Weapon          = 0x200, //0000_0010_0000_0000,
    Pistol,
    Rifle,
    Grenade,
    EndWeapon,
    
    Item            = 0x400, //0000_0100_0000_0000
    EndItem,
}

/*
 * 코드
 * [타입 number:1byte][ID number:1byte]
 * 타입 넘버 : 플래그 넘버. 하위 오브젝트는 플래그 연산을 통해 아이템 타입을 알아낼 수 있다.
 * ID 넘버   : 일반적인 정수값. 1byte이므로 0~255개의 id값을 가질 수 있음.
 */