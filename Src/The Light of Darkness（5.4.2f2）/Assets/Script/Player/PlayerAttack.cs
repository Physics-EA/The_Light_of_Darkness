using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerState
{
    ControlWalk,
    NormalAttack,
    SkillAttack,
    Death
}

/// <summary>
/// 攻击时候的状态
/// </summary>
public enum AttackState
{
    Moving,
    Idle,
    Attack
}


public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack _instance;

    public PlayerState state = PlayerState.ControlWalk;
    public AttackState attack_state = AttackState.Idle;

    public string aniname_normalattack;//普通攻击的动画
    public string aniname_idle;
    public string aniname_now;
    public float time_normalattack;//普通攻击的时间
    public float rate_normalattack = 1;
    private float timer = 0;
    public float min_distance = 5;//默认攻击的最小距离
    private Transform target_normalattack;

    private PlayerMove move;
    public GameObject effect;
    private bool showEffect = false;
    private PlayStatus ps;
    public float miss_rate = 0.25f;
    public GameObject hudtextPrefab;
    private GameObject hudtextFollow;
    private GameObject hudtextGo;
    private HUDText hudtext;
    public AudioClip miss_sound;
    public GameObject body;
    private Color normal;
    public string aniname_death;

    public GameObject[] efxArray;
    private Dictionary<string, GameObject> efxDict = new Dictionary<string, GameObject>();

    public bool isLockingTarget = false;//是否正在选择目标
    private SkillInfo info = null;

    void Awake()
    {
        _instance = this;
        move = this.GetComponent<PlayerMove>();
        ps = this.GetComponent<PlayStatus>();
        normal = body.GetComponent<Renderer>().material.color;

        hudtextFollow = transform.Find("HUDText").gameObject;

        foreach (GameObject go in efxArray)
        {
            efxDict.Add(go.name, go);
        }
    }

    void Start()
    {

        hudtextGo = NGUITools.AddChild(HUDTextParent._instance.gameObject, hudtextPrefab);

        hudtext = hudtextGo.GetComponent<HUDText>();
        UIFollowTarget followTarget = hudtextGo.GetComponent<UIFollowTarget>();
        followTarget.target = hudtextFollow.transform;
        followTarget.gameCamera = Camera.main;

    }

    void Update()
    {
        if (isLockingTarget == false && Input.GetMouseButtonDown(0) && state != PlayerState.Death)
        {
            //做射线检测
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);

            //当我们点击了一个敌人的时候
            if (isCollider && hitInfo.collider.tag == Tags.enemy)
            {

                target_normalattack = hitInfo.collider.transform;
                state = PlayerState.NormalAttack;//进入普通攻击的模式
                timer = 0;
                showEffect = false;
            }
            else
            {
                state = PlayerState.ControlWalk;
                target_normalattack = null;
            }
        }

        if (state == PlayerState.NormalAttack)
        {
            if (target_normalattack == null)
            {
                state = PlayerState.ControlWalk;
            }
            else
            {
                float distance = Vector3.Distance(transform.position, target_normalattack.position);

                //进行攻击
                if (distance <= min_distance)
                {
                    transform.LookAt(target_normalattack.position);
                    attack_state = AttackState.Attack;

                    timer += Time.deltaTime;
                    GetComponent<Animation>().CrossFade(aniname_now);
                    if (timer >= time_normalattack)
                    {
                        aniname_now = aniname_idle;
                        if (showEffect == false)
                        {
                            showEffect = true;
                            //播放特效
                            GameObject.Instantiate(effect, target_normalattack.position, Quaternion.identity); target_normalattack.GetComponent<WolfBaby>().TakeDamage(GetAttack());
                        }

                    }
                    if (timer >= (1f / rate_normalattack))
                    {
                        timer = 0;
                        showEffect = false;
                        aniname_now = aniname_normalattack;
                    }

                }
                //走向敌人
                else
                {
                    attack_state = AttackState.Moving;
                    move.SimpleMove(target_normalattack.position);
                }
            }
        }
        //如果死亡就播放死亡动画
        else if (state == PlayerState.Death)
        {
            GetComponent<Animation>().CrossFade(aniname_death);
        }

        if (isLockingTarget && Input.GetMouseButtonDown(0))
        {
            OnLockTarget();
        }
    }

    public int GetAttack()
    {
        return (int)(EquipmentUI._instance.attack + ps.attack + ps.attack_plus);
    }

    public void TakeDamage(int attack)
    {
        if (state == PlayerState.Death) return;
        float def = EquipmentUI._instance.def + ps.def + ps.def_plus;
        float temp = attack * ((200 - def) / 200);
        if (temp < 1) temp = 1;

        float value = Random.Range(0f, 1f);
        //MISS
        if (value < miss_rate)
        {
            AudioSource.PlayClipAtPoint(miss_sound, transform.position);
            hudtext.Add("MISS", Color.gray, 1);
        }
        else
        {
            hudtext.Add("-" + temp, Color.red, 1);
            ps.hp_remain -= (int)temp;
            StartCoroutine(ShowBodyRed());
            if (ps.hp_remain <= 0)
            {
                state = PlayerState.Death;
            }
        }

        HeadStatusUI._instance.UpdateShow();
    }

    IEnumerator ShowBodyRed()
    {
        body.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        body.GetComponent<Renderer>().material.color = normal;
    }

    void OnDestroy()
    {
        GameObject.Destroy(hudtextGo);
    }

    public void UseSkill(SkillInfo info)
    {
        if (ps.heroType == HeroType.Magician)
        {
            if (info.applicableRole == ApplicableRole.Swordman)
            {
                return;
            }
        }
        if (ps.heroType == HeroType.Swordman)
        {
            if (info.applicableRole == ApplicableRole.Magician)
            {
                return;
            }
        }
        switch (info.applyType)
        {
            case ApplyType.Passive:
                StartCoroutine(OnPassiveSkillUse(info));
                break;
            case ApplyType.Buff:
                StartCoroutine(OnBuffSkillUse(info));
                break;
            case ApplyType.SingleTarget://处理单个目标
                OnSingleTargetSkillUse(info);
                break;
            case ApplyType.MultiTarget:
                OnMultiTargetSkillUse(info);
                break;
        }

    }

    //处理增益技能
    IEnumerator OnPassiveSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        GetComponent<Animation>().CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);
        state = PlayerState.ControlWalk;
        int hp = 0, mp = 0;
        if (info.applyProperty == ApplyProperty.HP)
        {
            hp = info.applyValue;
        }
        else if (info.applyProperty == ApplyProperty.MP)
        {
            mp = info.applyValue;
        }

        ps.GetDrug(hp, mp);
        //实例化特效
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        GameObject.Instantiate(prefab, transform.position, Quaternion.identity);
    }

    //处理增强技能
    IEnumerator OnBuffSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        GetComponent<Animation>().CrossFade(info.aniname);
        yield return new WaitForSeconds(info.anitime);
        state = PlayerState.ControlWalk;

        //实例化特效
        GameObject prefab = null;
        efxDict.TryGetValue(info.efx_name, out prefab);
        GameObject.Instantiate(prefab, transform.position, Quaternion.identity);

        switch (info.applyProperty)
        {
            case ApplyProperty.Attack:
                ps.attack *= (info.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                rate_normalattack *= (info.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                ps.def *= (info.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                move.speed *= (info.applyValue / 100f);
                break;
        }
        yield return new WaitForSeconds(info.applyTime);
        switch (info.applyProperty)
        {
            case ApplyProperty.Attack:
                ps.attack /= (info.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                rate_normalattack /= (info.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                ps.def /= (info.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                move.speed /= (info.applyValue / 100f);
                break;
        }
    }

    //选择目标完成，开始技能的释放
    void OnLockTarget()
    {
        isLockingTarget = false;
        switch (info.applyType)
        {
            case ApplyType.SingleTarget:
                StartCoroutine(OnLockSingleTarget());
                break;
            case ApplyType.MultiTarget:
                StartCoroutine(OnLockMultiTarget());
                break;
        }
    }

    /// <summary>
    /// 选择单个目标
    /// </summary>
    /// <returns></returns>
    IEnumerator OnLockSingleTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isCollider = Physics.Raycast(ray, out hitInfo);
        if (isCollider && hitInfo.collider.tag == Tags.enemy)
        {//选择了一个敌人
            GetComponent<Animation>().CrossFade(info.aniname);
            yield return new WaitForSeconds(info.anitime);
            state = PlayerState.ControlWalk;
            //实例化特效
            GameObject prefab = null;
            efxDict.TryGetValue(info.efx_name, out prefab);
            GameObject.Instantiate(prefab, hitInfo.collider.transform.position, Quaternion.identity);

            hitInfo.collider.GetComponent<WolfBaby>().TakeDamage((int)(GetAttack() * (info.applyValue / 100f)));
        }
        else
        {
            state = PlayerState.NormalAttack;
        }
        CursorManager._instance.SetNormal();
    }

    /// <summary>
    /// 单个目标释放技能
    /// </summary>
    /// <param name="info"></param>
    void OnSingleTargetSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        CursorManager._instance.SetLockTarget();
        isLockingTarget = true;
        this.info = info;
    }

    /// <summary>
    /// 选择群体目标
    /// </summary>
    /// <returns></returns>
    IEnumerator OnLockMultiTarget()
    {
        CursorManager._instance.SetNormal();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isCollider = Physics.Raycast(ray, out hitInfo, 11);
        if (isCollider)
        {
            GetComponent<Animation>().CrossFade(info.aniname);
            yield return new WaitForSeconds(info.anitime);
            state = PlayerState.ControlWalk;

            //实例化特效
            GameObject prefab = null;
            efxDict.TryGetValue(info.efx_name, out prefab);
            GameObject go = GameObject.Instantiate(prefab, hitInfo.point + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
            go.GetComponent<MagicSphere>().attack = GetAttack() * (info.applyValue / 100f);
        }
        else
        {
            state = PlayerState.ControlWalk;
        }

    }

    /// <summary>
    /// 群体释放技能
    /// </summary>
    /// <param name="info"></param>
    void OnMultiTargetSkillUse(SkillInfo info)
    {
        state = PlayerState.SkillAttack;
        CursorManager._instance.SetLockTarget();
        isLockingTarget = true;
        this.info = info;
    }



}
