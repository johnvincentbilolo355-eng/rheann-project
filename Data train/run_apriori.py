import pandas as pd
from mlxtend.frequent_patterns import apriori, association_rules

df = pd.read_csv("Transformed data.csv").dropna()

# Convert numeric features to categorical tags
transactions = pd.DataFrame()

transactions["GWA_LOW"] = df["gwa"] < 75
transactions["GWA_MED"] = (df["gwa"] >= 75) & (df["gwa"] < 85)
transactions["GWA_HIGH"] = df["gwa"] >= 85

transactions["ATT_LOW"] = df["overall_attendance_avg"] < 75
transactions["ATT_GOOD"] = df["overall_attendance_avg"] >= 75

transactions["LOW_SUBJECTS_2PLUS"] = df["low_subject_count"] >= 2
transactions["BORDERLINE_2PLUS"] = df["borderline_subject_count"] >= 2

transactions["AT_RISK"] = df["at_risk"] == 1

# Convert boolean to 1/0
transactions = transactions.astype(int)

# Apply Apriori
frequent_itemsets = apriori(transactions, min_support=0.05, use_colnames=True)

rules = association_rules(
    frequent_itemsets,
    metric="confidence",
    min_threshold=0.6
)

# Focus only rules predicting AT_RISK
rules = rules[rules["consequents"] == {"AT_RISK"}]

print(rules[["antecedents", "support", "confidence", "lift"]])

rules.to_csv("apriori_rules.csv", index=False)
print("Rules saved as apriori_rules.csv")