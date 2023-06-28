using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySelector
{
    public ScannerModule<Transform> scanner;
    public ClassifierModule<Transform> classifier;

    public EntitySelector(ScannerModule<Transform> scanner, ClassifierModule<Transform> classifier)
    {
        this.scanner = scanner;
        this.classifier = classifier;
    }

    public Transform ScanEntity()
    {
        Transform[] targets = scanner.Scan();
        return classifier.GetObject(targets, targets.Length);
    }

    public Transform[] ScanEntities()
    {
        Transform[] cols = scanner.Scan();
        return classifier.SortByPriority(cols, cols.Length);
    }

    public bool JustScan()
    {
        return scanner.Scan().Length == 0;
    }

    public bool CheckScanned(Transform target)
    {
        return classifier.Check(target);
    }
}
