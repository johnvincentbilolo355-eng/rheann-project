import json
import sys
from pathlib import Path

import joblib
import numpy as np


FEATURES = [
    "gwa",
    "low_subject_count",
    "borderline_subject_count",
    "low_attendance_subject_count",
    "min_attendance_rate",
]


def _fail(message: str, *, exit_code: int = 2):
    sys.stdout.write(json.dumps({"ok": False, "error": message}))
    sys.exit(exit_code)


def main() -> int:
    raw = sys.stdin.read()
    if not raw.strip():
        _fail("No JSON input provided on stdin")

    try:
        payload = json.loads(raw)
    except Exception as ex:
        _fail(f"Invalid JSON input: {ex}")

    try:
        row = [float(payload[name]) for name in FEATURES]
    except KeyError as ex:
        _fail(f"Missing feature: {ex}")
    except Exception as ex:
        _fail(f"Invalid feature value: {ex}")

    model_path = Path(__file__).resolve().parent / "rf_seepath_model.pkl"
    if not model_path.exists():
        _fail(f"Model file not found: {model_path}")

    try:
        model = joblib.load(model_path)
    except Exception as ex:
        _fail(f"Failed to load model: {ex}")

    X = np.array([row], dtype=float)

    try:
        if hasattr(model, "predict_proba"):
            prob = float(model.predict_proba(X)[0][1])
        else:
            prob = float(model.predict(X)[0])
    except Exception as ex:
        _fail(f"Model inference failed: {ex}")

    pred = 1 if prob >= 0.5 else 0

    sys.stdout.write(
        json.dumps(
            {
                "ok": True,
                "prob_at_risk": prob,
                "pred_at_risk": pred,
                "threshold": 0.5,
                "features": dict(zip(FEATURES, row)),
            }
        )
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
