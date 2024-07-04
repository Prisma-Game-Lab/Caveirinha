using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private string enemyName;
    private string[] vectorNames = { "Adriana", "Adriano", "Agostinho", "Alan", "Alba", "Alessandra", "Alexandre", "Al Pacino", "Aline", "Amanda", "Anderson", "Andr�", "Ang�lica", "Ant�nio", "Arnaldo", "Arthur", "Aur�lio", "Barbara", "Beatriz", "Berenice", "Bernadete", "Bernardo", "Bingus", "Bin", "Bipo", "Bob", "Bonni", "Br�gida", "Bruno", "Caiozinho", "Camila", "C�ndido", "Carla", "Carlos", "Carolina", "Caroline", "Catarina", "Charles", "Clari", "C�sar", "C�sar", "Cristina", "Daniel", "Daniela", "Danilo", "Davi", "Dave", "D�bora", "Delfina", "Destroyer", "Diego", "Diogo", "Dion�sia", "Douglas", "Ecrenemenon", "Eduardo", "Elaine", "Eliana", "Elisa", "Enzo", "Erick", "Eus�bio", "Evaristo", "Evandro", "Fabiano", "F�bio", "Fandango", "Felix", "Fernanda", "Fernando", "Firmina", "Flapio", "Fl�via", "Floppa", "Flarpo", "Fibonaccio", "Freddy", "Frederico", "Francisco", "Frovio", "Gabriel", "Gabriela", "Gabrielle", "Garen", "Garpaccio", "Gartando", "Genghis Khan", "Geiso", "Geoffrey", "Geremias", "Geraldo", "Germano", "Giovanna", "Giovanni", "Gilberto", "Ginkobiloba", "Godfrey", "Gojo", "Guilherme", "Guaraci", "Haykal", "Helena", "Henrique", "Herm�nia", "Hildebrando", "Higgsboson", "Hugo", "Igor", "Ingrid", "Iolanda", "Isabel", "Isabela", "Jaqueline", "Joaquim", "Joana", "Johan", "Jo�o", "Jonas", "Joshua", "J�bilo", "Juliana", "Juliano", "J�lio", "Kevin", "Karine", "La�s", "Larissa", "Leandro"
            , "Leonardo", "Leoc�dia", "Let�cia", "Lillia", "Lito", "Logarino", "Lope", "Lucas", "Luana", "Luciana", "Luisa", "Luiza", "Ludovico", "Magnus", "Madalena", "Manuel", "Marcela", "Marcelo", "Marcos", "Mariana", "Marina", "M�rio", "Marta", "Maur�cio", "Mauro", "Mazinho", "Matheus", "Miguel", "Milena", "Nana", "Nat�lia", "Nelson", "Nelson", "Nilson", "Nicanor", "Nicolau", "N�bia", "Oct�vio", "Odete", "Olaso", "Orangofrango", "Orestes", "Ossoz�", "Pantale�o", "Pamela", "Patricia", "Patr�cia", "Patrick", "Paulo", "Pedro", "Papyrus", "Pepperonio", "Pingala", "Planilho", "Rafaela", "Rafael", "Raimunda", "Raul", "Rebeca", "Renan", "Renata", "Renato", "Riclaudio", "Robert de Niro", "Roberto", "Robson", "Rodrigo", "Rodrigo", "Rodriguez", "Ryan Gosling", "Sabrina", "Samuel", "Sandra", "Sans", "Sancho", "Santiago", "Saul", "Sergio", "Severino", "Syndra", "Tatiana", "Teodora", "Tharc�sio", "Tripas", "T�lio", "Ubaldo", "Valdomiro", "Vagner", "Vin�cius", "Virg�nia", "Vit�ria", "Walderez", "Walter", "Wantuwilson", "Wenceslau", "Weslley", "Xenofonte", "Yara", "Yuri", "Zagreu", "Zeferino", "Zen�bio", "Z�" };
    [SerializeField]
    private string enemyClass;
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
    [SerializeField]
    int soulType;

    private void Awake()
    {
        enemyName = vectorNames[Random.Range(0, vectorNames.Length)];
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        GameObject soul = Instantiate(soulObject, transform.position, Quaternion.identity);
        soul.GetComponent<SoulScript>().inicialConfiguration(soulType,enemyName);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 directionVector = collision.transform.position - transform.position;
            collision.GetComponent<PlayerController>().TakeDamage(contactDamage);
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(knockbackStrenght * directionVector.normalized, ForceMode2D.Impulse);
        }
    }
}
