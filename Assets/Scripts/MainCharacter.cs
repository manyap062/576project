using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour {

    private Animator animation_controller;
    private CharacterController character_controller;
    public Vector3 movement_direction;
    public float max_velocity;
    public float velocity;

    public bool has_lost;
    public bool has_won;

    // Start
    void Start() {
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        movement_direction = new Vector3(0.0f, 0.0f, 0.0f);
        max_velocity = 2f;
        velocity = 0.0f;

        has_won = false;
        has_lost = false;  
    }

    // Update
    void Update() {
        bool walking_forward = false;
        bool walking_backward = false;
        bool running = false;              
        bool sneaking = false;

        // Update booleans based on what keys have been pressed

        // Update Running bool
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) { running = true; }

        // Update Sneaking bool
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftControl)) { sneaking = true; }

        // Update Walking Forward bool
        else if (Input.GetKey(KeyCode.W)) { walking_forward = true; }

        // Update Walking Backward bool
        else if (Input.GetKey(KeyCode.S)) { walking_backward = true; }
        

        // Update Animation Controller
        animation_controller.SetBool("walking_forward", walking_forward);
        animation_controller.SetBool("walking_backward", walking_backward);
        animation_controller.SetBool("running", running);
        animation_controller.SetBool("sneaking", sneaking);
    
        // Update Movement Vector & Velocity
        if (has_lost == false && has_won == false) {
            
            // Walking Forward logic
            if (animation_controller.GetBool("walking_forward") == true) {
                velocity += 0.1f;
                if (velocity > max_velocity) { velocity = max_velocity; }
            }

            // Walking Backward logic
            else if (animation_controller.GetBool("walking_backward") == true) {
                if (velocity > 0) { velocity = velocity * -1f; }
                velocity -= 0.1f;
                if (-velocity > (max_velocity / 1.5f)) { velocity = -(max_velocity / 1.5f); }
            }

            // Running logic
            else if (animation_controller.GetBool("running") == true) {
                Debug.Log("should be running");
                velocity += 0.1f;
                if (velocity > (max_velocity * 1.2f)) { velocity = (max_velocity * 1.2f); }
            }

            // Sneaking logic
            else if (animation_controller.GetBool("sneaking") == true) {
                Debug.Log("should be sneaking");
                velocity += 0.1f;
                if (velocity > (max_velocity * 0.5f)) { velocity = (max_velocity * 0.5f); }
            }

            // Returning to idle logic
            else { 
                if (velocity > 0.0f) { velocity -= 0.2f; }
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
            lower_character.y = -10f; 
            character_controller.Move(lower_character);
        }

        else { character_controller.Move(movement_direction * velocity * Time.deltaTime); }  
    }
}
