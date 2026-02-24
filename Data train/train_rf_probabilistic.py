import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import classification_report, confusion_matrix, accuracy_score
import joblib

df = pd.read_csv("Transformed data.csv").dropna()

X = df[[
    "gwa",
    "low_subject_count",
    "borderline_subject_count",
    "low_attendance_subject_count",
    "min_attendance_rate"
]]

y = df["at_risk"]

X_train, X_test, y_train, y_test = train_test_split(
    X, y,
    test_size=0.2,
    random_state=42,
    stratify=y
)

model = RandomForestClassifier(
    n_estimators=600,
    max_depth=12,
    min_samples_leaf=2,
    random_state=42
)

model.fit(X_train, y_train)

y_pred = model.predict(X_test)

acc = accuracy_score(y_test, y_pred)

print("\n===== MODEL PERFORMANCE =====")
print("Accuracy:", round(acc * 100, 2), "%\n")
print("Classification Report:\n")
print(classification_report(y_test, y_pred))
print("Confusion Matrix:\n")
print(confusion_matrix(y_test, y_pred))

joblib.dump(model, "rf_seepath_model.pkl")
print("\nModel saved.")