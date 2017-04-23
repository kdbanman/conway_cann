# conway_cann

cellular automatic neural network that learns the classic conway's ruleset


### TODO

- make the neural net ring trainable (whee math!)
    - training sorta works now. should fix above and try more stuff to be sure.
    - everything still converges to 0.5. :-(
    - I feel like zero input shouldn't have been vanishing the gradients!
    - I feel like zero input shouldn't have been vanishing the gradients!
    - I feel like zero input shouldn't have been vanishing the gradients!
    - I feel like zero input shouldn't have been vanishing the gradients!
- train ring on touch while grabbing
    - this ontriggerstay intersection stuff seems really slow with no training at all yet
        - probably optimize the dumb shit away (GetComponent?)
        - probably thread the training
- only advance the ring being held.
- add weight inspector panels
- add cost panel
- speed the damned thing up. frames are so slow.
- make another reference ring
- add a floor