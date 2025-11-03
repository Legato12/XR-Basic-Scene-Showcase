# XR Showcase — Quest 3 / OpenXR (Unity)

Hands-on VR showcase for Meta Quest 3 using **OpenXR** and **XR Interaction Toolkit**.  
Focus: simple runtime, reliable interactions, and one‑click Editor automation.

![Alt text](Scene.png)

Video Demo: https://youtube.com/shorts/o8yd5Ivjf-o

---

## Highlights

- **One‑click setup.** Menu actions wire puzzles and clean the scene.
- **Solid interactions.** Push/pull with ray stick, two‑hand scaling, no physics jitter.
- **Readable code.** “Rookie‑style” runtime but 100% working; editor scripts do the heavy lifting.

---

## Table index (scene has 4 pedestals)

1. **Basics** — Grab, rotate with wrist, throw. No custom scripts.
2. **Color/ID Socket Puzzle** — Place the right shapes into sockets to open a door.  
   - `TokenId` on pieces  
   - `SocketMatchPuzzle` on the pedestal  
   - Optional `SocketExpectId` on each socket to declare required ID
3. **Slider Puzzle** — Three sliders with bulbs; hit 25% / 50% / 75% to open a drawer and play fireworks.  
   - `AxisSlider` (non‑physics)  
   - `SliderStatusLight` (green/yellow)  
   - `SliderPuzzleOrchestrator` (drawer + lamp + FX)
4. **Two‑Hand Scaling** — Grab the object with two controllers and scale by changing distance.  
   - `TwoHandScaler` (`XRGrabInteractable.selectMode = Multiple`)

**Quality of life**
- `GrabJoystickManipulator` on the **right Ray** disables rig turning while holding so the thumbstick drives **Anchor Control** (push/pull), then restores turning on release.
- `BillboardLabel` keeps table labels facing the player.

---

## Quick start

1. **Clone** and open in Unity 2022.3+ (URP or Built‑in; tested on Quest 3).
2. Open `Scenes/XR_Showcase.unity`.
3. Run **XR Showcase → MASTER: Finalize Showcase (minimal, clean)**.  
   This wires all tables, enables continuous turn, sets up ray push/pull, and self‑deletes.
4. (Optional) **XR Showcase → One‑Click: Patch (Sockets + TwoHand + Labels)** if you imported the patch set later.
5. Press Play in editor, or build for Android.

---

## Controls

- **Right stick**: smooth turn when not grabbing.  
- **While holding with Ray**: right‑stick up/down = push/pull (Anchor Control), left/right = rig turning disabled.  
- **Two‑hand scale**: grab with both controllers and move hands apart/together.  
- **Single‑hand optional scale**: assign stick Y in `TwoHandScaler.singleHandY`.

---

## Scripts (final minimal set)

- **Core interactions**  
  `GrabJoystickManipulator`, `BillboardLabel`

- **Socket puzzle**  
  `TokenId`, `SocketExpectId` (optional), `SocketMatchPuzzle`

- **Slider puzzle**  
  `AxisSlider`, `SliderStatusLight`, `SliderPuzzleOrchestrator`

- **Two‑hand demo**  
  `TwoHandScaler`

Generated materials live under `Assets/XR_Showcase/Generated/` (safe to ignore in git).

---

## Build (Quest 3)

1. Install **Android Build Support**.  
2. Project Settings → **XR Plug‑in Management** → **OpenXR** for Android.  
3. Switch Platform to **Android**, build as `.apk`/`.aab`.

---

## Why this project

- I automate VR setups with **Editor tooling** so designers get one‑click builds.
- I keep runtime scripts **small and explicit**, favoring clarity over cleverness.
- I aim for **robust interaction** (no physics twitching, predictable inputs).

If you’re hiring for Unity XR, this repository shows I ship fast, with reliable UX and maintainable code.

---
