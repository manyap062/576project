using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour {

    private Animator animation_controller;
    private CharacterController character_controller;
    public Vector3 movement_direction;
    public float max_velocity;
    public float velocity;

    // Condition Boolean Values
    public bool has_won;                // TODO: Implement later
    public bool has_lost;               // TODO: Implement later
    public bool recently_hit;           // TODO: Implement later

    // Start
    void Start() {
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        movement_direction = new Vector3(0.0f, 0.0f, 0.0f);
        max_velocity = 1.5f;
        velocity = 0.0f;

        has_won = false;
        has_lost = false;  
        recently_hit = false; 
    }

    // Update
    void Update() {
        bool walking_forward = false;
        bool walking_backward = false;
        bool sneaking_forward = false;     // TODO: Implement later
        bool sneaking_backward = false;    // TODO: Implement later
        bool running = false;              // TODO: Implement later
        bool petting = false;              // TODO: Implement later

        // Update Boolean Values
        if (Input.GetKey(KeyCode.W)) {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { running = true; }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) { sneaking_forward = true; }
            else { walking_forward = true; }
        }

        else if (Input.GetKey(KeyCode.S)) {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) { sneaking_backward = true; }
            else { walking_backward = true; }
        }

        // Update Animation Controller
        animation_controller.SetBool("walking_forward", walking_forward);
        animation_controller.SetBool("walking_backward", walking_backward);
    
        // Update Movement Vector & Velocity
        if (has_lost == false && has_won == false) {
            
            if (animation_controller.GetBool("walking_forward") == true) {
                velocity += 0.1f;
                if (velocity > max_velocity) { velocity = max_velocity; }
            }

            else if (animation_controller.GetBool("walking_backward") == true) {
                if (velocity > 0) { velocity = velocity * -1f; }
                velocity -= 0.1f;
                if (-velocity > (max_velocity / 1.5f)) { velocity = -(max_velocity / 1.5f); }
            }

            else { 
                if (velocity > 0.0f) { velocity -= 0.1f; }
                else { velocity = 0.0f; } 
            }
            

            // Rotating Camera using A/D Keys
            if (Input.GetKey(KeyCode.A)) { transform.Rotate(new Vector3(0.0f, -0.5f, 0.0f)); }
            else if (Input.GetKey(KeyCode.D)) { transform.Rotate(new Vector3(0.0f, 0.5f, 0.0f)); }
        }
    

        // Execute movement
        float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        movement_direction = new Vector3(xdirection, 0.0f, zdirection);

        if (transform.position.y > 0.0f) { 
            Vector3 lower_character = movement_direction * velocity * Time.deltaTime;
            lower_character.y = -100f; 
            character_controller.Move(lower_character);
        }

        else { character_controller.Move(movement_direction * velocity * Time.deltaTime); }  
    }
}
