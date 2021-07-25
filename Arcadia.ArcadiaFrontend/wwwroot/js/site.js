function DistanceFormatter(data) {
	if (data == null || isNaN(data))
		return null;

	var distance = parseFloat(data).toLocaleString(undefined, {
		minimumFractionDigits: 3,
		maximumFractionDigits: 3
	});
	return distance + " km";
};
