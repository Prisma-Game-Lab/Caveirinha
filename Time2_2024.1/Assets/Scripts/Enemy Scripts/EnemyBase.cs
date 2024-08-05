using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    BlinkScript blinkScript;
    EnemyManager enemyManager;

    private string enemyName;

    private string[] vectorNames = { "Adriana", "Adriano", "Agostinho", "Alan", "Alba", "Alessandra", "Alexandre",
                                     "Al Pacino", "Aline", "Amanda", "Anderson", "André", "Angélica", "Antônio",
                                     "Arnaldo", "Arthur", "Aurélio", "Barbara", "Beatriz", "Berenice", "Bernadete",
                                     "Bernardo", "Bingus", "Bini", "Bipo", "Bob", "Bonni", "Brigida", "Bruno", 
                                     "Caiozinho", "Camila", "Cândido", "Carla", "Carlos", "Carolina", "Caroline",
                                     "Catarina", "Charles", "Clari", "César", "Cristina", "Daniel", 
                                     "Daniela", "Danilo", "Davi", "Dave", "Débora", "Delfina", "Destroyer", "Diego", 
                                     "Diogo", "Dionísio", "Douglas", "Ecrenemenon", "Eduardo", "Elaine", "Eliana", 
                                     "Elisa", "Enzo", "Erick", "Eusébio", "Evaristo", "Evandro", "Fabiano", "Fábio", 
                                     "Fandango", "Felix", "Fernanda", "Fernando", "Firmina", "Flapio", "Flávia", "Floppa", 
                                     "Flarpo", "Fibonaccio", "Freddy", "Frederico", "Francisco", "Frovio", "Gabriel", 
                                     "Gabriela", "Gabrielle", "Garen", "Garpaccio", "Gartando", "Genghis Khan", "Geiso", 
                                     "Geoffrey", "Geremias", "Geraldo", "Germano", "Giovanna", "Giovanni", "Gilberto", 
                                     "Ginkobiloba", "Godfrey", "Gojo", "Guilherme", "Guaraci", "Hannah", "Haykal", "Helena", 
                                     "Henrique", "Hermônia", "Hildebrando", "Higgsboson", "Hugo", "Igor", "Ingrid", 
                                     "Iolanda", "Isabel", "Isabela", "Jaqueline", "Joaquim", "Joana", "Johan", "João", 
                                     "Jonas", "Joshua", "Júbilo", "Juliana", "Juliano", "Júlio", "Ken", "Kevin", "Karine", 
                                     "Laís", "Larissa", "Leandro", "Leonardo", "Leocádia", "Letícia", "Lillia", "Lito",
                                     "Logarino", "Lope", "Lucas", "Luana", "Luciana", "Luisa", "Luiza", "Ludovico", "Magnus",
                                     "Madalena", "Manuel", "Marcela", "Marcelo", "Marcos", "Mariana", "Marina", "Mário", 
                                     "Marta", "Maurício", "Mauro", "Mazinho", "Matheus", "Miguel", "Milena", "Nana", 
                                     "Natália", "Nelson", "Nelson", "Nilson", "Nicanor", "Nicolau", "Nubia", "Octávio",
                                     "Odete", "Olaso", "Orangofrango", "Orestes", "Ossozé", "Pantaleo", "Pamela", "Patricia",
                                     "Patrícia", "Patrick", "Paulo", "Pedro", "Papyrus", "Pepperonio", "Pingala", "Planilho", 
                                     "Rafaela", "Rafael", "Raimunda", "Raul", "Rebeca", "Renan", "Renata", "Renato", "Riclaudio",
                                     "Robert de Niro", "Roberto", "Robson", "Rodrigo", "Rodrigo", "Rodriguez", "Ryan Gosling",
                                     "Sabrina", "Samuel", "Sandra", "Sans", "Sancho", "Santiago", "Saul", "Sergio", "Severino", "Shaka", "Suzy",
                                     "Syndra", "Tatiana", "Teodora", "Tharcísio", "Tripas", "Túlio", "Ubaldo", "Valdomiro", "Vagner",
                                     "Vinícius", "Virgínia", "Vitória", "Walderez", "Walter", "Wantuwilson", "Wenceslau", "Weslley",
                                     "Xenofonte", "Yara", "Yuri", "Zagreu", "Zorba", "Zeferino", "Zenóbio", "Zé", "Ofonildo", "Momonga", "Tritri", "Ludwig", "Klaus", "Jorel",
                                     "Anai", "Simone", "Eide", "Olivia", "Gelda", "Az", "Aster", "Killia", "Valvatorez", "Pierogi", "Chalupa", "Zucchini", "Pistacchio", "Lamar", "Phill", 
                                     "Frieren", "Himmel", "Cocytus", "Kiryu", "Majima", "Daigo", "Guadalupe", "Guantano", "Morbius", "Ferdinando", "Abner", "Xerxes", "Lorenzo", "Weng",
                                     "Xiao", "Billy", "Mia", "Jeniffer", "Debora", "Honda", "Jimmy", "Chandler", "Connor", "Chambara", "Chamito", "Taco", "Melina", "Ranni", "Remi", "Dolores", "Zell",
                                     "Jordan", "Sonic", "Miku", "Markimoo", "Otávio", "Osvaldo", "Ralof", "Alduin", "Keanu Reeves", "Beowulf", "Wolfgang", "Corki", "Anubis", "Mike", "Frederica",
                                     "Festanzo", "Clara", "Chong", "Lebron", "Larry", "Xu", "Aquiles", "Chapelone", "Garbanzo", "Brad", "Krogmar", "Fuji", "Xube", "Bonzi", "Frango", "Michel", "Tio Fred",
                                     "Tony", "Sebastian", "Garfield", "Lance"};

    [SerializeField] private float[] healthVector;
    [SerializeField] private float[] attackVector;
    [SerializeField] private float contactDamage;
    [SerializeField] private float knockbackStrenght;
    [SerializeField] GameObject soulObject;
    [SerializeField] [Tooltip("0 - Vida, 1 - Dano, 2 - Speed")] int soulType;
    [SerializeField] private string onDeathAudio;

    private float currentHealth;
    protected float currentEnemyAttack;

    private void Awake()
    {
        currentHealth = healthVector[GameManager.instance.Floor];
        currentEnemyAttack = attackVector[GameManager.instance.Floor];
        enemyName = vectorNames[Random.Range(0, vectorNames.Length)];
        blinkScript = GetComponent<BlinkScript>();
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    public void TakeDamage(float damage)
    {
        AudioManager.instance.PlaySFX("HIT");
        currentHealth -= damage;
        StartCoroutine(blinkScript.Blink());
        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySFX(onDeathAudio);
            OnDeath();
        }
    }

    void OnDeath()
    {
        GameObject soul = Instantiate(soulObject, transform.position, Quaternion.identity);
        soul.GetComponent<SoulScript>().inicialConfiguration(soulType,enemyName);
        enemyManager.OnEnemyDeath();
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 directionVector = collision.transform.position - transform.position;
            bool hit = collision.GetComponent<PlayerController>().TakeDamage(contactDamage, directionVector.normalized, knockbackStrenght);
            if (soulType == 1 && hit)
            {
                AudioManager.instance.PlaySFX("SHIT");
            }
        }
    }
}
