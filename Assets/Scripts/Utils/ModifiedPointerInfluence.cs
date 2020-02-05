/*
 void ApplyInfluence(float deltaTime)
        {
            
            bool keyboardInputReceived = CalculateKeyboardMovement();
            if (!keyboardInputReceived)
            {
                ApplyPointerMovement(deltaTime);
            }
        }

        bool CalculateKeyboardMovement()
        {
            float ScrollSpeed = 2f;
            float hInfluence = 0;
            float vInfluence = 0;
            float horizontalInfluence = 0;
            float verticalInfluence = 0;
        
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInfluence -= 1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                verticalInfluence -= 1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInfluence += 1;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                verticalInfluence += 1;
            }

            Vector2 influenceVector = new Vector2(horizontalInfluence, verticalInfluence);

            if (influenceVector != Vector2.zero)
            {
                ProCamera2D.ApplyInfluence(influenceVector.normalized * ScrollSpeed);
                return true;
            }

            return false;
        }

        void ApplyPointerMovement(float deltaTime)
        {
            var mousePosViewport = ProCamera2D.GameCamera.ScreenToViewportPoint(Input.mousePosition);

            var mousePosViewportH = mousePosViewport.x.Remap(0, 1, -1, 1);
            var mousePosViewportV = mousePosViewport.y.Remap(0, 1, -1, 1);

            float hInfluence = 0;
            float vInfluence = 0;
            if (mousePosViewportH <= -.9f || mousePosViewportH >= .9f)
            {
                hInfluence  = mousePosViewportH * MaxHorizontalInfluence;
            }
            if (mousePosViewportV <= -.9f || mousePosViewportV >= .9f)
            {
                vInfluence = mousePosViewportV * MaxVerticalInfluence;
            }

            _influence = Vector2.SmoothDamp(_influence, new Vector2(hInfluence, vInfluence), ref _velocity, InfluenceSmoothness, Mathf.Infinity, deltaTime);

            if (_influence != Vector2.zero)
            {
                ProCamera2D.ApplyInfluence(_influence);
            }
        }
*/