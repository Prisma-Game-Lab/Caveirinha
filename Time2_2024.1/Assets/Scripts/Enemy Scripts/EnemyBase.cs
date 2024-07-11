using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    Rigidbody2D rb;

    private string enemyName;

    private string[] vectorNames = { "Adriana", "Adriano", "Agostinho", "Alan", "Alba", "Alessandra", "Alexandre",
                                     "Al Pacino", "Aline", "Amanda", "Anderson", "André", "Angélica", "Antônio",
                                     "Arnaldo", "Arthur", "Aurélio", "Barbara", "Beatriz", "Berenice", "Bernadete",
                                     "Bernardo", "Bingus", "Bini", "Bipo", "Bob", "Bonni", "Brígida", "Bruno", 
                                     "Caiozinho", "Camila", "Cândido", "Carla", "Carlos", "Carolina", "Caroline",
                                     "Catarina", "Charles", "Clari", "César", "César", "Cristina", "Daniel", 
                                     "Daniela", "Danilo", "Davi", "Dave", "Débora", "Delfina", "Destroyer", "Diego", 
                                     "Diogo", "Dionísia", "Douglas", "Ecrenemenon", "Eduardo", "Elaine", "Eliana", 
                                     "Elisa", "Enzo", "Erick", "Eusébio", "Evaristo", "Evandro", "Fabiano", "Fábio", 
                                     "Fandango", "Felix", "Fernanda", "Fernando", "Firmina", "Flapio", "Flávia", "Floppa", 
                                     "Flarpo", "Fibonaccio", "Freddy", "Frederico", "Francisco", "Frovio", "Gabriel", 
                                     "Gabriela", "Gabrielle", "Garen", "Garpaccio", "Gartando", "Genghis Khan", "Geiso", 
                                     "Geoffrey", "Geremias", "Geraldo", "Germano", "Giovanna", "Giovanni", "Gilberto", 
                                     "Ginkobiloba", "Godfrey", "Gojo", "Guilherme", "Guaraci", "Hannah", "Haykal", "Helena", 
                                     "Henrique", "Hermínia", "Hildebrando", "Higgsboson", "Hugo", "Igor", "Ingrid", 
                                     "Iolanda", "Isabel", "Isabela", "Jaqueline", "Joaquim", "Joana", "Johan", "João", 
                                     "Jonas", "Joshua", "Júbilo", "Juliana", "Juliano", "Júlio", "Ken", "Kevin", "Karine", 
                                     "Laís", "Larissa", "Leandro", "Leonardo", "Leocádia", "Letícia", "Lillia", "Lito",
                                     "Logarino", "Lope", "Lucas", "Luana", "Luciana", "Luisa", "Luiza", "Ludovico", "Magnus",
                                     "Madalena", "Manuel", "Marcela", "Marcelo", "Marcos", "Mariana", "Marina", "Mário", 
                                     "Marta", "Maurício", "Mauro", "Mazinho", "Matheus", "Miguel", "Milena", "Nana", 
                                     "Natália", "Nelson", "Nelson", "Nilson", "Nicanor", "Nicolau", "Núbia", "Octávio",
                                     "Odete", "Olaso", "Orangofrango", "Orestes", "Ossozé", "Pantaleão", "Pamela", "Patricia",
                                     "Patrícia", "Patrick", "Paulo", "Pedro", "Papyrus", "Pepperonio", "Pingala", "Planilho", 
                                     "Rafaela", "Rafael", "Raimunda", "Raul", "Rebeca", "Renan", "Renata", "Renato", "Riclaudio",
                                     "Robert de Niro", "Roberto", "Robson", "Rodrigo", "Rodrigo", "Rodriguez", "Ryan Gosling",
                                     "Sabrina", "Samuel", "Sandra", "Sans", "Sancho", "Santiago", "Saul", "Sergio", "Severino", "Shaka", "Suzy",
                                     "Syndra", "Tatiana", "Teodora", "Tharcísio", "Tripas", "Túlio", "Ubaldo", "Valdomiro", "Vagner",
                                     "Vinícius", "Virgínia", "Vitória", "Walderez", "Walter", "Wantuwilson", "Wenceslau", "Weslley",
                                     "Xenofonte", "Yara", "Yuri", "Zagreu", "Zorba", "Zeferino", "Zenóbio", "Zé" };

    [SerializeField]
    private float health;
    [SerializeField]
    private float enemyAttack;
    [SerializeField]
    private float contactDamage;
    [SerializeField]
    private float knockbackStrenght;
    [SerializeField]
    GameObject soulObject;
    [SerializeField] [Tooltip("0 - Vida, 1 - Dano, 2 - Speed")]
    int soulType;

    private void Awake()
    {
        enemyName = vectorNames[Random.Range(0, vectorNames.Length)];
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection, float knockback)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath();
        }
        rb.AddForce(knockback * knockbackDirection, ForceMode2D.Impulse);
    }

    void OnDeath()
    {
        GameObject soul = Instantiate(soulObject, transform.position, Quaternion.identity);
        soul.GetComponent<SoulScript>().inicialConfiguration(soulType,enemyName);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 directionVector = collision.transform.position - transform.position;
            collision.GetComponent<PlayerController>().TakeDamage(contactDamage, directionVector.normalized, knockbackStrenght);
        }
    }
}
