# 🚜 Excavator Simulator – sEMG Based HMI Using Deep Learning

A cost-effective, wearable **surface EMG (sEMG)–based Human Machine Interface (HMI)** designed to control a virtual excavator in Unity.
This project integrates hardware (EMG + FSR + ESP32), signal processing, and deep learning to create an immersive VR training system for industrial machine operators.

## 🔌 Arduino File (Hardware + Data Acquisition)

**Purpose:**
Handles real-time acquisition of muscle signals (sEMG) and pressure input (FSR), processes raw signals, and transmits classified outputs.

**Key Responsibilities:**

* Reads analog signals from:

  * EMG Biosensor Module
  * Force-Sensing Resistor (FSR)
* Converts signals into structured data
* Sends processed signals to Unity (via ESP32 WiFi/Bluetooth)
* Exports sensor readings as CSV for ML training

**Hardware Used:**

* EMG Sensor Module (3 electrodes)
* FSR Pad
* ESP32 Microcontroller
* 9V Power Supply

The Arduino layer forms the **signal acquisition and preprocessing pipeline** of the system.

---

## 🎮 Unity File (Simulation Environment)

**Purpose:**
Implements the virtual excavator training environment and maps classified sensor signals to excavator movements.

**Key Features:**

* Custom excavator avatar
* Real-time arm and bucket movement control
* Binary signal mapping logic:

  * EMG = muscle activation
  * FSR = pressure intensity
* Context-sensitive excavation behavior
* Interactive VR-ready simulation

**Control Logic Example:**

* EMG = 1, FSR = 1 → Arm down + Dig
* EMG = 1, FSR = 0 → Arm down
* EMG = 0, FSR = 1 → Dig only
* EMG = 0, FSR = 0 → Idle state

Unity serves as the **visualization and operator training interface**.

---

## 🧠 Deep Learning Notebook (Model Training & Evaluation)

**Platform Used:** Jupyter Notebook / Python

**Purpose:**
Trains deep learning models to classify EMG and FSR signals.

**Workflow:**

1. Import CSV sensor datasets
2. Signal preprocessing:
3. Model architecture experimentation
4. Performance comparison

### Models Implemented:

* Convolutional Neural Networks (CNN)
* Multiple configurations:

  * 1 Layer (32 neurons)
  * 2 Layers (64 neurons)
  * 3 Layers (128 neurons)

### Performance Summary:

* EMG Accuracy ≈ 71%
* FSR Accuracy ≈ 98%
* Combined System Accuracy ≈ 84.5%

The notebook includes:

* Model training code
* Performance metrics
* Accuracy comparison graphs
* Architecture optimization experiments

This component represents the **intelligence layer** of the system.

---

## ⚠️ Current Limitation

Due to time constraints, the trained deep learning model was **not embedded directly into Unity** for real-time inference.

Instead:

* Classification was performed externally.
* Binary outputs were transmitted to Unity for control logic.

Future work will focus on:

* Exporting trained models (e.g., ONNX / TensorFlow Lite)
* Integrating real-time inference inside Unity
* Multi-channel sEMG expansion
* Improved model generalization

---

## 🎯 Project Objective

To develop a **low-cost, indigenous alternative** to expensive haptic gloves and armbands for VR-based excavator operator training — improving safety, reducing training costs, and enhancing accessibility in industrial sectors.

---

## 📄 License

Academic / Research Use

---

If you are exploring **EMG-based control systems, VR training simulators, sensor fusion, or embedded ML systems**, this repository demonstrates a complete hardware-to-simulation pipeline.
